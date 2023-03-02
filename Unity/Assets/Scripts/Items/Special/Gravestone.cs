using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravestone : PermanentItem
{
    private string[] dialogue = null;
    private AudioClip[] clips = null;

    private string audio_path = null;
    public override void UseItem () {

        
        if(!DialogueSystem.the().english){
            this.audio_path = "Audio/DE/";
            this.dialogue = new string[] {"Ich frage mich wirklich, wer hier begraben ist.",
                                          "Muss wohl jemand wichtiges gewesen sein bei diesem schönen Grab.",
                                          "Vielleicht hat diese Person einmal über diese Insel geherrscht? Wer weiß...",
                                          "Na gut, ich sollte weiter machen, damit ich meine Familie finde und hoffentlich ergeht es Ihnen nicht so wie dieser Person hier..."};
        } else {
            this.audio_path = "Audio/EN/";
            this.dialogue = new string[] {"I really wonder who lies here.",
                                          "It seems to be someone important because this grave is very beautiful.",
                                          "Maybe this person once ruled this island? Who knows...",
                                          "Anyways, I should move on so I can finally meet my family and hopefully not like that..."};
        }

        this.clips = new AudioClip[] {Resources.Load<AudioClip>(audio_path + "Gravestone1"), 
                                      Resources.Load<AudioClip>(audio_path + "Gravestone2"),
                                      Resources.Load<AudioClip>(audio_path + "Gravestone3"),
                                      Resources.Load<AudioClip>(audio_path + "Gravestone4")};
            
        DialogueSystem.the().StartDialogue(dialogue, clips);
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
