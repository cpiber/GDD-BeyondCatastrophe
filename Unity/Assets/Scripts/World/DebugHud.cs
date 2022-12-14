using UnityEngine;

public class DebugHud : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text hud;

    void Update() {
        var d = DayNightSystem.the();
        var s = SeasonSystem.the();
        var t = TemperatureSystem.the();
        var p = StatusSystem.the();
        hud.text = @$"Current time: <b>{d.TimeInDay.ToString("N1")}</b>/{d.GetParams.secondsPerDay} of day <b>{d.Day+1}</b> ({d.SectionInDay})
Season: <b>{s.CurrentSeason}</b> (day {s.DayInYear + 1} of year)
Temperature: <b>{t.Temperature.ToString("N1")}</b>
Body Temperature: <b>{p.EffectiveBodyTemperature.ToString("N1")}</b>/{p.BodyTemperature.ToString("N1")} (target {p.TargetBodyTemperature.ToString("N1")})
Status: health <b>{p.Health}</b>/100 - tiredness <b>{p.Tiredness.ToString("N1")}</b>/100 - energy <b>{p.Energy.ToString("N1")}</b>/100";
    }
}
