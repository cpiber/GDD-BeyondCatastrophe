using System.Collections;
using UnityEngine;

public class WinterIslandPrompt : MonoBehaviour
{
    private string[] dialogue = null;
    private AudioClip[] clips = null;
    private string audio_path = null;

    void OnTriggerEnter2D(Collider2D collider) {
        if(DialogueSystem.the().IsOpen) return;
        if (collider.tag == "Player") {
           if(!DialogueSystem.the().english){
            this.audio_path = "Audio/DE/";
            this.dialogue = new string[] {"Der Telefonmast! Ich muss da unten hin und ihn mit Strom versorgen.",
                                          "Vielleicht kann ich sogar irgendwo Solarpanels für mich selbst auch finden."};
            } else {
                this.audio_path = "Audio/EN/";
                this.dialogue = new string[] {"The phone mast! I need to go down there and give it some electricity.",
                                              "Maybe I can even get some solar panels for myself..."};
            }

            this.clips = new AudioClip[] {Resources.Load<AudioClip>(audio_path + "Winterisland1"), 
                                        Resources.Load<AudioClip>(audio_path + "Winterisland2")};
            
                StartCoroutine(ShowDialogueAndDestroy());
        }
    }
    
    private IEnumerator ShowDialogueAndDestroy() {
        yield return StartCoroutine(DialogueSystem.the().StartDialogueRoutine(dialogue, clips));
        GetComponent<SceneObjectState>().Destroy();
    }
}
