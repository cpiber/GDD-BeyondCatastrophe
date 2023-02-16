using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravestone : PermanentItem
{
    private string[] computer_dialogue = {"I really wonder who lies here.",
                                          "It seems to be someone important because this grave is very beautiful.",
                                          "Maybe this person once ruled this island? Who knows...",
                                          "Anyways, I should move on so I can finally meet my family and hopefully not like that..."};
    private AudioClip[] clips = null;
    public override void UseItem () {
        if(this.clips == null){
            this.clips = new AudioClip[] {Resources.Load<AudioClip>("Audio/test1"),
                                          Resources.Load<AudioClip>("Audio/test1"),
                                          Resources.Load<AudioClip>("Audio/test1"),
                                          Resources.Load<AudioClip>("Audio/test1")};
        }
    
        DialogueSystem.the().StartDialogue(computer_dialogue, clips);
    }

    public override string GetItemName() {
        return "Gravestone";
    }

    public override string GetItemDescription() {
        return "This is a Gravestone.";
    }

    public override bool IsInteractible() {
        return true;
    }

    public override bool IsCollectible() {
        return false;
    }
}
