using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : PermanentItem
{

    /**

Player: Has the possibility to walk around his room and explore the environment. When going near items, they are prompted with the buttons to interact and pick up (this is turned off after leaving the house).

Upon leaving the room he sees the telecommunication mast on the right of the screen (island). The idea now is to wait for the water to freeze in order walk there.

This is introduced by another short scene with dialogue: "The phone mast! I need to go over there and give it some electricity. Maybe I can even get some solar panels for myself..."
    */
    private string[] computer_dialogue = {"I don't have any communication since the electric grid broke down.", 
                                        "I'm worried that my family isn't okay. I have to go and find them.", 
                                        "But this isn't as easy as it used to be, winters and nights are very cold, summers are too hot, I don't have any electricity. I have to be careful with my resources on my journey.",
                                        "God damn climate crisis..."};
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
        return "Computer";
    }

    public override string GetItemDescription() {
        return "This is the computer of the programmer.";
    }

    public override bool IsInteractible() {
        return true;
    }

    public override bool IsCollectible() {
        return false;
    }
}
