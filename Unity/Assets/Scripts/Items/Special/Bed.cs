using System.Collections;
using UnityEngine;

public class Bed : PermanentItem
{
    [SerializeField] float bedTimeSeconds = 2f;

    public override void UseItem () {
        // TODO disallow sleeping when not tired
        StartCoroutine(Sleep());
    }

    IEnumerator Sleep() {
        InventoryUIManager.the().CloseAllUI();
        yield return StartCoroutine(DayNightSystem.the().GoToSleep());
        SeasonSystem.the().AllowUpdateSeason();
        switch (DayNightSystem.the().SectionInDay) {
            case DayNightSystem.DaySection.Night:
            case DayNightSystem.DaySection.Dusk:
                DayNightSystem.the().AdvanceToDawn();
                break;
            default:
                DayNightSystem.the().AdvanceToDusk();
                break;
        }
        StatusSystem.the().Sleep();
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
