using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndoorPlant : PermanentItem
{
    private string[] dialogue = null;
    private AudioClip[] clips = null;

    private string audio_path = null;
    public override void UseItem () {

        
        if(!DialogueSystem.the().english){
            this.audio_path = "Audio/DE/";
            this.dialogue = new string[] {"Es ist schwer zu glauben das Pflanzen kaum mehr wachsen...",
                                          "Auf diese Insel zu leben ist nahezu wie ein Traum...",
                                          "Aber ich sollte jetzt wirklich versuchen meine familie zu finden selbst wenn das bedeutet das ich hier weg muss..."};
        } else {
            this.audio_path = "Audio/EN/";
            this.dialogue = new string[] {"Its hard to believe that plants hardly grow anymore...",
                                          "Living on this island is like a dream...",
                                          "But i should really find my family even if this means i have to leave here!"};
        }

        this.clips = new AudioClip[] {Resources.Load<AudioClip>(audio_path + "Indoorplant1"), 
                                      Resources.Load<AudioClip>(audio_path + "Indoorplant2"),
                                      Resources.Load<AudioClip>(audio_path + "Indoorplant3")};
            
        DialogueSystem.the().StartDialogue(dialogue, clips);
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
