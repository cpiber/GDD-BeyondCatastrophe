using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndoorPlant : PermanentItem
{
    private string[] computer_dialogue = {"Its hard to belive that plants hardly grow anymore...",
                                          "Living on this island is like a dream...",
                                          "But i should really find my family even if this means i have to leave here!"  };
    private AudioClip[] clips = null;
    public override void UseItem () {
        if(this.clips == null){
            this.clips = new AudioClip[] {Resources.Load<AudioClip>("Audio/test1"),
                                          Resources.Load<AudioClip>("Audio/test1"),
                                          Resources.Load<AudioClip>("Audio/test1")};
        }
    
        DialogueSystem.the().StartDialogue(computer_dialogue, clips);
    }

    public override string GetItemName() {
        return "IndoorPlant";
    }

    public override string GetItemDescription() {
        return "This is a Plant.";
    }

    public override bool IsInteractible() {
        return true;
    }

    public override bool IsCollectible() {
        return false;
    }
}
