using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayNightSystem : GenericSingleton<DayNightSystem>
{
    [Serializable]
    public struct DayNightParams {
        public int secondsPerDay;
        [Range(0, 1)] public float darknessPercent;
        [Range(0, 1)] public float darknessEasingPercent;
        public int DarknessSeconds => (int) (secondsPerDay * darknessPercent);
        public int DarknessEasingSeconds => (int) (secondsPerDay * darknessEasingPercent);

        public DayNightParams(int secondsPerDay, float darknessPercent, float darknessEasingPercent) {
            this.secondsPerDay = secondsPerDay;
            this.darknessPercent = darknessPercent;
            this.darknessEasingPercent = darknessEasingPercent;
        }
    }

    [SerializeField] float time = 0;
    [SerializeField] DayNightParams dayNightParams = new DayNightParams(5 * 60, 0.3f, 0.08f);
    [SerializeField] [Range(0, 1)] float minBrightness = 0.2f;
    [SerializeField] float fadeTimeFactor = 0.5f;
    public float MinBrightness => minBrightness;
    public float Brightness => brightnessCurve.Evaluate(time);

    public float Time => time;
    public int Day => (int) time / dayNightParams.secondsPerDay;
    public float TimeInDay => time % dayNightParams.secondsPerDay;
    public DayNightParams GetParams => dayNightParams;
    [SerializeField] bool paused = false;

    [SerializeField] private AnimationCurve brightnessCurve = null;
    [SerializeField] private Light2D globalLight = null;

    public enum DaySection {
        Night,
        Dawn,
        Day,
        Dusk,
    }
    public DaySection SectionInDay {
        get {
            float b = Brightness;
            if (Mathf.Approximately(b, minBrightness)) return DaySection.Night;
            if (Mathf.Approximately(b, 1)) return DaySection.Day;
            if (TimeInDay < dayNightParams.secondsPerDay / 2) return DaySection.Dawn;
            return DaySection.Dusk;
        }
    }

    void Start() {
        time = dayNightParams.secondsPerDay / 2; // midday
        ComputeCurve();
    }

    void FixedUpdate() {
        if (!paused) {
            time += UnityEngine.Time.deltaTime;
            globalLight.intensity = Brightness;
        }
    }

    [ContextMenu("Recompute Curve")]
    void ComputeCurve() {
        var secondsPerDay = dayNightParams.secondsPerDay;
        var darknessPercent = dayNightParams.darknessPercent;
        var darknessEasingPercent = dayNightParams.darknessEasingPercent;
        Debug.Log($"Recomputing brightness curve with minBrightness={minBrightness}, darknessPercent={darknessPercent}, darknessEasingPercent={darknessEasingPercent}, secondsPerDay={secondsPerDay}");
        brightnessCurve = ComputeDayCurve(secondsPerDay, minBrightness, 1, darknessPercent, darknessEasingPercent);
    }

    public static AnimationCurve ComputeDayCurve(float duration, float vMin, float vMax, float darknessPercent, float darknessEasingPercent) {
        var curve = new AnimationCurve(
            new Keyframe(0, vMin), // midnight
            new Keyframe((darknessPercent - darknessEasingPercent) / 2 * duration, vMin), // to dawn (dark)
            new Keyframe((darknessPercent + darknessEasingPercent) / 2 * duration, vMax), // to dawn (light)
            new Keyframe((1 - (darknessPercent + darknessEasingPercent) / 2) * duration, vMax), // to dusk (light)
            new Keyframe((1 - (darknessPercent - darknessEasingPercent) / 2) * duration, vMin), // to dusk (dark)
            new Keyframe(duration, vMin) // midnight
        );
        curve.preWrapMode = WrapMode.Loop;
        curve.postWrapMode = WrapMode.Loop;
        return curve;
    }

    public IEnumerator GoToSleep() {
        paused = true;
        // darken to 0
        while (globalLight.intensity > 0) {
            globalLight.intensity = Mathf.Max(0.0f, globalLight.intensity - UnityEngine.Time.deltaTime * fadeTimeFactor);
            yield return null;
        }
    }

    public IEnumerator WakeUp() {
        Debug.Assert(paused, "Script relies on intensity being free to manipulate");
        // go to regular brightness
        while (globalLight.intensity < Brightness) {
            globalLight.intensity = Mathf.Min(Brightness, globalLight.intensity + UnityEngine.Time.deltaTime * fadeTimeFactor);
            yield return null;
        }
        paused = false;
    }

    public void AdvanceToTimeInDay(float targetTime) {
        if (targetTime > TimeInDay) time += targetTime - TimeInDay;
        else time += ((float) dayNightParams.secondsPerDay - TimeInDay) + targetTime;
    }
    public void AdvanceToDusk() {
        AdvanceToTimeInDay((1 - dayNightParams.darknessPercent / 2) * (float) dayNightParams.secondsPerDay);
    }
    public void AdvanceToDawn() {
        AdvanceToTimeInDay(dayNightParams.darknessPercent / 2 * (float) dayNightParams.secondsPerDay);
    }

#if UNITY_EDITOR
    void OnValidate() {
        ComputeCurve();
    }
#endif
}
