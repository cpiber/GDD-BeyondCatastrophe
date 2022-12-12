using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayNightSystem : GenericSingleton<DayNightSystem>
{
    [SerializeField] float time = 0;
    [SerializeField] int secondsPerDay = 5 * 60;
    [SerializeField] [Range(0, 1)] float darknessPercent = 0.3f;
    [SerializeField] [Range(0, 1)] float darknessEasingPercent = 0.08f;
    [SerializeField] [Range(0, 1)] float minBrightness = 0.2f;
    [SerializeField] float fadeTimeFactor = 0.5f;
    public float MinBrightness => minBrightness;
    public float Brightness => brightnessCurve.Evaluate(time);

    public float Time => time;
    public int Day => (int) time / secondsPerDay;
    public float TimeInDay => time % secondsPerDay;
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
            if (TimeInDay < secondsPerDay / 2) return DaySection.Dawn;
            return DaySection.Dusk;
        }
    }

    void Start() {
        time = secondsPerDay / 2; // midday
        ComputeCurve();
    }

    void FixedUpdate() {
        if (!paused) {
            time += UnityEngine.Time.deltaTime;
            globalLight.intensity = Brightness;
        }
    }

    void ComputeCurve() {
        Debug.Log($"Recomputing curve with minBrightness={minBrightness}, darknessPercent={darknessPercent}, darknessEasingPercent={darknessEasingPercent}, secondsPerDay={secondsPerDay}");
        brightnessCurve = new AnimationCurve(
            new Keyframe(0, minBrightness), // midnight
            new Keyframe((darknessPercent - darknessEasingPercent) / 2 * (float) secondsPerDay, minBrightness), // to dawn (dark)
            new Keyframe((darknessPercent + darknessEasingPercent) / 2 * (float) secondsPerDay, 1), // to dawn (light)
            new Keyframe((1 - (darknessPercent + darknessEasingPercent) / 2) * (float) secondsPerDay, 1), // to dusk (light)
            new Keyframe((1 - (darknessPercent - darknessEasingPercent) / 2) * (float) secondsPerDay, minBrightness), // to dusk (dark)
            new Keyframe((float) secondsPerDay, minBrightness) // midnight
        );
        brightnessCurve.preWrapMode = WrapMode.Loop;
        brightnessCurve.postWrapMode = WrapMode.Loop;
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
        // go to regular brightness
        while (globalLight.intensity < Brightness) {
            globalLight.intensity = Mathf.Min(Brightness, globalLight.intensity + UnityEngine.Time.deltaTime * fadeTimeFactor);
            yield return null;
        }
        paused = false;
    }

    public void AdvanceToTimeInDay(float targetTime) {
        if (targetTime > TimeInDay) time += targetTime - TimeInDay;
        else time += ((float) secondsPerDay - TimeInDay) + targetTime;
    }
    public void AdvanceToDusk() {
        AdvanceToTimeInDay((1 - darknessPercent / 2) * (float) secondsPerDay);
    }
    public void AdvanceToDawn() {
        AdvanceToTimeInDay(darknessPercent / 2 * (float) secondsPerDay);
    }

#if UNITY_EDITOR
    void OnValidate() {
        ComputeCurve();
    }
#endif
}
