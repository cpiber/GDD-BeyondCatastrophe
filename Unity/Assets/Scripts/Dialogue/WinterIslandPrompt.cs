using System.Collections;
using UnityEngine;

public class WinterIslandPrompt : MonoBehaviour
{
    private string[] dialogue = {"The phone mast! I need to go down there and give it some electricity.",
                                 "Maybe I can even get some solar panels for myself..."};
    private AudioClip[] clips = null;

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "Player") {
            if(this.clips == null){
                this.clips = new AudioClip[] {Resources.Load<AudioClip>("Audio/test1"), 
                                              Resources.Load<AudioClip>("Audio/test1")};
            }
        
            StartCoroutine(ShowDialogueAndDestroy());
        }
    }
    
    private IEnumerator ShowDialogueAndDestroy() {
        yield return StartCoroutine(DialogueSystem.the().StartDialogueRoutine(dialogue, clips));
        GetComponent<SceneObjectState>().Destroy();
    }
}
