using System;
using UnityEngine;
using UnityEngine.Events;

public class SeasonSystem : GenericSingleton<SeasonSystem>
{
    [Serializable]
     public class SeasonChangeEvent : UnityEvent<Seasons> { }

    [SerializeField] int daysPerSeason = 20;
    [SerializeField] int transitionDays = 2;
    [SerializeField] Seasons lastSeason = Seasons.Fall;
    public SeasonChangeEvent OnSeasonChange;

    public enum Seasons {
        Fall,
        Winter,
        Spring,
        Summer,
    }

    public int DayInYear => DayNightSystem.the().Day % (daysPerSeason * 2 + transitionDays * 2);
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
    /// As opposed to <seealso cref="CalculatedSeason"/>, this will not change during the "day". See <see cref="AllowUpdateSeason"/>.
    /// </remarks>
    public Seasons CurrentSeason => lastSeason;

    public void AllowUpdateSeason() {
        var newSeason = CalculatedSeason;
        if (newSeason != lastSeason) OnSeasonChange.Invoke(newSeason);
        lastSeason = newSeason;
    }
}
