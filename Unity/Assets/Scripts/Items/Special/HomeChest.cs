using System.Collections;
using UnityEngine;

public class HomeChest : PermanentItem
{
    private const string key = "Startroom.HomeChest";

    private string[] dialogue = null;
    private AudioClip[] clips = null;

    private string audio_path = null;

    void Start() {
        var state = GlobalSceneState.the().getState(key);
        if (state != null && !state.exists) Destroy(this);
    }

    public override void UseItem () {
        // TODO: check for language flag
        if(false){
            this.audio_path = "Audio/DE/";
            this.dialogue = new string[] {"Mein ganzes Zeug ist weg. Wieso mussten sie mich ausrauben...",
                                          "Zumindest habe ich noch einen kleine Vorrat damit ich nicht verhungere."};
        } else {
            this.audio_path = "Audio/EN/";
            this.dialogue = new string[] {"All my stuff is gone. Why did they have to rob me now...",
                                          "At least I still got some stashes, so I won't starve."};
        }

        this.clips = new AudioClip[] {Resources.Load<AudioClip>(audio_path + "Homechest1"), 
                                      Resources.Load<AudioClip>(audio_path + "Homechest2")};
    
        StartCoroutine(ShowDialogueAndOpenActualChest());
    }

    private IEnumerator ShowDialogueAndOpenActualChest() {
        yield return StartCoroutine(DialogueSystem.the().StartDialogueRoutine(dialogue, clips));
        GetComponent<Chest>().UseItem();
        Destroy(this);
        GlobalSceneState.the().setExists(key, false);
    }

    public override string GetItemName() {
        return "Chest";
    }

    public override string GetItemDescription() {
        return "A classic chest which can hold items.";
    }

    public override bool IsInteractible() {
        return true;
    }

    public override bool IsCollectible() {
        return false;
    }
}
