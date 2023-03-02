using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftRightSign : PermanentItem
{
    private string[] dialogue = null;
    private AudioClip[] clips = null;

    private string audio_path = null;
    public override void UseItem () {

        
        if(!DialogueSystem.the().english){
            this.audio_path = "Audio/DE/";
            this.dialogue = new string[] {"Willkommen auf Destiny Island.",
                                          "Wähle deinen Weg weise und falle nicht auf Versuchungen rein... "};
        } else {
            this.audio_path = "Audio/EN/";
            this.dialogue = new string[] {"Welcome to Destiny Island.",
                                          "Choose your path wisely and don't be fooled by whatever might try to do so..."};
        }

        this.clips = new AudioClip[] {Resources.Load<AudioClip>(audio_path + "LeftRightSign1"), 
                                      Resources.Load<AudioClip>(audio_path + "LeftRightSign2")};
            
        DialogueSystem.the().StartDialogue(dialogue, clips);
    }

    public override string GetItemName() {
        return "LeftRightSign";
    }

    public override string GetItemDescription() {
        return "This is the sign of destiny";
    }

    public override bool IsInteractible() {
        return true;
    }

    public override bool IsCollectible() {
        return false;
    }
}

