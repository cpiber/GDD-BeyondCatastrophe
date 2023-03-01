using System.Collections;
using UnityEngine;

public class HomeChest : PermanentItem
{
    private const string key = "Startroom.HomeChest";
    [SerializeField] Item[] itemsToGive;

    private string[] dialogue = null;
    private AudioClip[] clips = null;

    private string audio_path = null;

    void Start() {
        var state = GlobalSceneState.the().getState(key);
        if (state != null && !state.exists) Destroy(this);

        StartCoroutine(SetOutlineObject());
    }

    private IEnumerator SetOutlineObject() {
        yield return new WaitForFixedUpdate();
        outlineObject = GetComponent<Chest>().OutlineObject;
    }

    public override void UseItem () {
        if(DialogueSystem.the().IsOpen) return;
        
        foreach (var item in itemsToGive) {
            InventoryManager.the().AddSlotItem(item, InventoryManager.the().GetAllItemSlotsChestFirst());
        }

        
        if(!DialogueSystem.the().english){
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
        PlayerController.the().UnregisterCollectItem(this);
        PlayerController.the().RegisterCollectItem(GetComponent<Chest>());
        GlobalSceneState.the().setExists(key, false);
        Destroy(this);
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
