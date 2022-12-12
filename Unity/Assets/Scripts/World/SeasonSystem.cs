using UnityEngine;

public class SeasonSystem : GenericSingleton<SeasonSystem>
{
    [SerializeField] DayNightSystem dayNightSystem;
    [SerializeField] int daysPerSeason = 20;
    [SerializeField] int transitionDays = 2;
    [SerializeField] Seasons lastSeason = Seasons.Fall;

    public enum Seasons {
        Fall,
        Winter,
        Spring,
        Summer,
    }

    public int DayInYear => dayNightSystem.Day % (daysPerSeason * 2 + transitionDays * 2);
    public Seasons CalculatedSeason {
        get {
            int d = DayInYear;
            if (d < transitionDays) return Seasons.Fall;
            d -= transitionDays;
            if (d < daysPerSeason) return Seasons.Winter;
            d -= daysPerSeason;
            if (d < transitionDays) return Seasons.Spring;
            d -= transitionDays;
            Debug.Assert(d < daysPerSeason, "Should not be reached");
            return Seasons.Summer;
        }
    }
    /// <summary>
    /// The currently active season
    /// </summary>
    /// <remarks>
    /// As opposed to CalculatedSeason, this will not change during the "day". See <see cref="AllowUpdateSeason"/>.
    /// </remarks>
    public Seasons CurrentSeason => lastSeason;

    void Awake() {
        dayNightSystem = this.GetComponent<DayNightSystem>();
    }

    public void AllowUpdateSeason() {
        lastSeason = CalculatedSeason;
    }
    
#if UNITY_EDITOR
    void OnValidate() {
        dayNightSystem = this.GetComponent<DayNightSystem>();
    }
#endif
}
