using System.Collections;
using UnityEngine;

public class Bed : PermanentItem
{
    [SerializeField] float bedTimeSeconds = 2f;

    public override void UseItem () {
        StartCoroutine(Sleep());
    }

    IEnumerator Sleep() {
        // TODO disable moving
        yield return StartCoroutine(DayNightSystem.the().GoToSleep());
        SeasonSystem.the().AllowUpdateSeason();
        if (DayNightSystem.the().SectionInDay == DayNightSystem.DaySection.Night) DayNightSystem.the().AdvanceToDawn();
        else DayNightSystem.the().AdvanceToDusk();
        yield return new WaitForSeconds(bedTimeSeconds);
        yield return StartCoroutine(DayNightSystem.the().WakeUp());
    }

    public override string GetItemName() {
        return "Bed";
    }

    public override string GetItemDescription() {
        return "A comfy get to sleep in.";
    }

    public override bool IsInteractible() {
        return true;
    }

    public override bool IsCollectible() {
        return false;
    }
}
