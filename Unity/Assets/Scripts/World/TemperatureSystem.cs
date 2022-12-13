using UnityEngine;

public class TemperatureSystem : GenericSingleton<TemperatureSystem>
{
    [SerializeField] float fallBaseTemperature = 5f;
    [SerializeField] float winterBaseTemperature = -15f;
    [SerializeField] float springBaseTemperature = 15f;
    [SerializeField] float summerBaseTemperature = 30f;
    [SerializeField] float dayTemperatureOffsetMin = -15f;
    [SerializeField] float dayTemperatureOffsetMax = 15f;
    [SerializeField] [Min(0)] float easingMultiplier = 2f;

    public float Temperature => temperatureCurve.Evaluate(DayNightSystem.the().Time);

    [SerializeField] private AnimationCurve temperatureCurve = null;

    void Start() {
        SeasonSystem.the().OnSeasonChange.AddListener(ComputeCurve);
        ComputeCurve();
    }

    float BaseTemperature(SeasonSystem.Seasons season) {
        switch (season) {
        case SeasonSystem.Seasons.Fall: return fallBaseTemperature;
        case SeasonSystem.Seasons.Winter: return winterBaseTemperature;
        case SeasonSystem.Seasons.Spring: return springBaseTemperature;
        case SeasonSystem.Seasons.Summer: return summerBaseTemperature;
        default:
            Debug.Assert(false);
            return 0;
        }
    }
    
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
