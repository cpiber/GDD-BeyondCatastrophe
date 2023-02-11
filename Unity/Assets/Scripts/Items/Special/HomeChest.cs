using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeChest : PermanentItem
{
    private string[] computer_dialogue = {"All my stuff is gone. Why did they have to rob me now...",
                                          "At least I still got some stashes, so I won't starve."};
    private AudioClip[] clips = null;

    public override void UseItem () {
         if(this.clips == null){
            this.clips = new AudioClip[] {Resources.Load<AudioClip>("Audio/test1"), 
                                          Resources.Load<AudioClip>("Audio/test1"),};
        }
    
        DialogueSystem.the().StartDialogue(computer_dialogue, clips);
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
