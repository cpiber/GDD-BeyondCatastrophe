using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Couch : PermanentItem
{
    private string[] computer_dialogue = {"I should probably look for my family instead of sitting..."};
    private AudioClip[] clips = null;
    public override void UseItem () {
        if(this.clips == null){
            this.clips = new AudioClip[] {Resources.Load<AudioClip>("Audio/test1")};
        }
    
        DialogueSystem.the().StartDialogue(computer_dialogue, clips);
    }

    public override string GetItemName() {
        return "Couch";
    }

    public override string GetItemDescription() {
        return "This is a couch.";
    }

    public override bool IsInteractible() {
        return true;
    }

    public override bool IsCollectible() {
        return false;
    }
}
