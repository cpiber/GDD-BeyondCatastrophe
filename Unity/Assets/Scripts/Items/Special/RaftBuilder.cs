using UnityEngine;

public class RaftBuilder : PermanentItem
{
    [SerializeField] string globalKey = "RaftBuilder";
    [SerializeField] GameObject raft;

    private string[] dialogue = null;
    private AudioClip[] clips = null;
    private string audio_path = null;

    void Start() {
        var state = GlobalSceneState.the().getState(globalKey);
        if (state != null && !state.exists) Destroy(this);
        SetSprite();
    }

    public override void UseItem() {
        if (InventoryManager.the().TakeBagItem("WoodLogs", 4) != null) {
            raft.SetActive(true);
            GlobalSceneState.the().setExists(globalKey, false);
            PlayerController.the().UnregisterCollectItem(this);
            Destroy(this);
        } else {
           if(false){
                this.audio_path = "Audio/DE/";
                this.dialogue = new string[] {"Ich brauche wohl noch ein paar mehr Hölzer, um das Floß zu bauen..."};
            } else {
                this.audio_path = "Audio/EN/";
                this.dialogue = new string[] {"Looks like I still need some more logs to build that raft..."};
            }

            this.clips = new AudioClip[] {Resources.Load<AudioClip>(audio_path + "Familyisland2")};
            DialogueSystem.the().StartDialogue(dialogue, clips);
        }
    }

    public override string GetItemName() {
        return "Raft Builder";
    }

    public override string GetItemDescription() {
        return "";
    }

    public override bool IsInteractible() {
        return true;
    }

    public override bool IsCollectible() {
        return false;
    }
}
