using System.Collections;
using UnityEngine;

public class Bed : PermanentItem
{
    [SerializeField] DayNightSystem dayNightSystem;
    [SerializeField] SeasonSystem seasonSystem;
    [SerializeField] float bedTimeSeconds = 2f;

    public override void UseItem () {
        StartCoroutine(Sleep());
    }

    IEnumerator Sleep() {
        // TODO disable moving
        yield return StartCoroutine(dayNightSystem.GoToSleep());
        seasonSystem.AllowUpdateSeason();
        // TODO dayNightSystem.AdvanceToTimeInDay()
        yield return new WaitForSeconds(bedTimeSeconds);
        yield return StartCoroutine(dayNightSystem.WakeUp());
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
