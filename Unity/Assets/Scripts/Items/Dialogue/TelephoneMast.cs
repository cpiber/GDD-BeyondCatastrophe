using UnityEngine;

public class TelephoneMast : PermanentItem
{
    private string[] dialogue = {"ring ring ring... No one picks up."};
    private AudioClip[] clips = null;

    public override void UseItem () {
        if(this.clips == null){
            this.clips = new AudioClip[] {Resources.Load<AudioClip>("Audio/test1")};
        }
    
        DialogueSystem.the().StartDialogue(dialogue, clips);
    }

    public override string GetItemName() {
        return "Telephone Mast";
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
