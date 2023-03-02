using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Couch : PermanentItem
{
    private string[] dialogue = null;
    private AudioClip[] clips = null;

    private string audio_path = null;
    public override void UseItem () {

        
        if(!DialogueSystem.the().english){
            this.audio_path = "Audio/DE/";
            this.dialogue = new string[] {"Ich sollte lieber nach meiner Familie suchen, anstatt hier zu sitzen..."};
        } else {
            this.audio_path = "Audio/EN/";
            this.dialogue = new string[] {"I should probably look for my family instead of sitting..."};
        }

        this.clips = new AudioClip[] {Resources.Load<AudioClip>(audio_path + "Couch1"), };
            
        DialogueSystem.the().StartDialogue(dialogue, clips);
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
