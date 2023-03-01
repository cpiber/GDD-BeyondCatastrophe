using System.Collections;
using UnityEngine;

public class FamilyIslandPrompt : MonoBehaviour
{
    private string[] dialogue = null;
    private AudioClip[] clips = null;
    private string audio_path = null;

    void OnTriggerEnter2D(Collider2D collider) {
        if(DialogueSystem.the().IsOpen) return;
        if (collider.tag == "Player") {
           if(!DialogueSystem.the().english){
                this.audio_path = "Audio/DE/";
                this.dialogue = new string[] {"Das sieht nach einem super Platz für ein Floß aus"};
            } else {
                this.audio_path = "Audio/EN/";
                this.dialogue = new string[] {"That looks like a good spot to build a raft"};
            }

            this.clips = new AudioClip[] {Resources.Load<AudioClip>(audio_path + "FamilyIsland1")};
            
            StartCoroutine(ShowDialogueAndDestroy());
        }
    }
    
    private IEnumerator ShowDialogueAndDestroy() {
        yield return StartCoroutine(DialogueSystem.the().StartDialogueRoutine(dialogue, clips));
        GetComponent<SceneObjectState>().Destroy();
    }
}
