using System.Collections;
using UnityEngine;

public class HomeChest : PermanentItem
{
    private const string key = "Startroom.HomeChest";

    private string[] computer_dialogue = {"All my stuff is gone. Why did they have to rob me now...",
                                          "At least I still got some stashes, so I won't starve."};
    private AudioClip[] clips = null;

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
        if(this.clips == null){
            this.clips = new AudioClip[] {Resources.Load<AudioClip>("Audio/test1"), 
                                          Resources.Load<AudioClip>("Audio/test1"),};
        }
    
        StartCoroutine(ShowDialogueAndOpenActualChest());
    }

    private IEnumerator ShowDialogueAndOpenActualChest() {
        yield return StartCoroutine(DialogueSystem.the().StartDialogueRoutine(computer_dialogue, clips));
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
