using System;
using UnityEngine;

public class TemperatureSystem : GenericSingleton<TemperatureSystem>
{
    [SerializeField] PlayerController player;

    [SerializeField] float fallBaseTemperature = 5f;
    [SerializeField] float winterBaseTemperature = -15f;
    [SerializeField] float springBaseTemperature = 15f;
    [SerializeField] float summerBaseTemperature = 30f;
    [SerializeField] float dayTemperatureOffsetMin = -15f;
    [SerializeField] float dayTemperatureOffsetMax = 15f;
    [SerializeField] [Min(0)] float easingMultiplier = 2f;

    // TODO this should consider if we're in a heated room -> add colliders and heat sources
    public float Temperature => player.CurrentRoom != null ? player.CurrentRoom.Heating : OutsideTemperature;
    public float OutsideTemperature => temperatureCurve.Evaluate(DayNightSystem.the().Time);

    [SerializeField] private AnimationCurve temperatureCurve = null;

    void Start() {
        SeasonSystem.the().OnSeasonChange.AddListener(ComputeCurve);
        ComputeCurve();
    }

    float BaseTemperature(SeasonSystem.Seasons season) {
        return season switch {
            SeasonSystem.Seasons.Fall   => fallBaseTemperature,
            SeasonSystem.Seasons.Winter => winterBaseTemperature,
            SeasonSystem.Seasons.Spring => springBaseTemperature,
            SeasonSystem.Seasons.Summer => summerBaseTemperature,
            _ => throw new ArgumentOutOfRangeException(nameof(season), $"Unexpected season value {season}"),
        };
    }
    
    [ContextMenu("Recompute Curve")]
    void ComputeCurve() {
        ComputeCurve(SeasonSystem.the().CurrentSeason);
    }
    void ComputeCurve(SeasonSystem.Seasons season) {
        var p = DayNightSystem.the().GetParams;
        var secondsPerDay = p.secondsPerDay;
        var darknessPercent = p.darknessPercent;
        var darknessEasingPercent = p.darknessEasingPercent * easingMultiplier;
        var baseTemp = BaseTemperature(season);
        var minTemp = baseTemp + dayTemperatureOffsetMin;
        var maxTemp = baseTemp + dayTemperatureOffsetMax;
        Debug.Log($"Recomputing temperature curve with baseTemp={baseTemp}, darknessPercent={darknessPercent}, darknessEasingPercent={darknessEasingPercent}, secondsPerDay={secondsPerDay}");
        temperatureCurve = DayNightSystem.ComputeDayCurve(secondsPerDay, minTemp, maxTemp, darknessPercent, darknessEasingPercent);
    }

#if UNITY_EDITOR
    void OnValidate() {
        SeasonSystem.the().OnSeasonChange.AddListener(ComputeCurve);
        ComputeCurve();
    }
#endif
}
