using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : PermanentItem
{
    private string[] dialogue = null;
    private AudioClip[] clips = null;

    private string audio_path = null;
    public override void UseItem () {

        
        if(!DialogueSystem.the().english){
            this.audio_path = "Audio/DE/";
            this.dialogue = new string[] {"Ich habe keine möglichkeit zu kommunizieren weil die Stromversorgung zusammengebrochen ist.",
                                          "Langsam mache ich mir sorgen das es meiner Familie nicht gut geht. Ich sollte sie finden.",
                                          "Ich sollte schnell meine Tochter finden damit ich mit ihr die Insel verlassen kann um nach dem rest der Familie zu sehen.",
                                          "Das ist aber nicht so leicht wie es mal war da die Winter und die Nächte sehr kalt sind und die Sommer viel zu warm. Ich habe keinen Strom. Ich muss auf meine Ressourcen achten auf meiner Reise.",
                                          "Verfluchte Klimakriese..."};
        } else {
            this.audio_path = "Audio/EN/";
            this.dialogue = new string[] {"I don't have any communication since the electric grid broke down.", 
                                          "I'm worried that my family isn't okay. I have to go and find them.",
                                          "I should find my daughter so that we can leave this island and look for the rest of our family.", 
                                          "But this isn't as easy as it used to be, winters and nights are very cold, summers are too hot, I don't have any electricity. I have to be careful with my resources on my journey.",
                                          "God damn climate crisis..."};
        }

        this.clips = new AudioClip[] {Resources.Load<AudioClip>(audio_path + "Computer1"), 
                                      Resources.Load<AudioClip>(audio_path + "Computer2"),
                                      Resources.Load<AudioClip>(audio_path + "Computer3"),
                                      Resources.Load<AudioClip>(audio_path + "Computer4"),
                                      Resources.Load<AudioClip>(audio_path + "Computer5")};
            
        DialogueSystem.the().StartDialogue(dialogue, clips);
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
