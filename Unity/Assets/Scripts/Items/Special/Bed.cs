using System.Collections;
using UnityEngine;

public class Bed : PermanentItem
{
    [SerializeField] float bedTimeSeconds = 2f;
    [SerializeField] int minTiredness = StatusSystem.STATUS_MAX / 4;

    private string[] dialogue = null;
    private AudioClip[] clips = null;

    private string audio_path = null;
    public override void UseItem () {

        
        if(!DialogueSystem.the().english){
            this.audio_path = "Audio/DE/";
            this.dialogue = new string[] {"Was mach ich da, ich bin noch nichtmal wirklich müde."};
        } else {
            this.audio_path = "Audio/EN/";
            this.dialogue = new string[] {"Ugh, what am I doing, I'm not even tired."};
        }

        this.clips = new AudioClip[] {Resources.Load<AudioClip>(audio_path + "Bed1")};
            
        if (StatusSystem.STATUS_MAX - StatusSystem.the().Tiredness < minTiredness) DialogueSystem.the().StartDialogue(dialogue, clips);
        else StartCoroutine(Sleep());
    }

    IEnumerator Sleep() {
        InventoryUIManager.the().CloseAllUI();
        yield return StartCoroutine(DayNightSystem.the().GoToSleep());
        switch (DayNightSystem.the().SectionInDay) {
            case DayNightSystem.DaySection.Night:
            case DayNightSystem.DaySection.Dusk:
                DayNightSystem.the().AdvanceToDawn();
                break;
            default:
                DayNightSystem.the().AdvanceToDusk();
                break;
        }
        SeasonSystem.the().AllowUpdateSeason();
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
