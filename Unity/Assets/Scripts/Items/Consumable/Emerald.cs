using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emerald : PermanentItem
{
    private string[] dialogue = null;
    private AudioClip[] clips = null;

    private string audio_path = null;
    public override void UseItem () {
        if(!DialogueSystem.the().english){
            this.audio_path = "Audio/DE/";
            this.dialogue = new string[] {"Irgendwie fühle ich mich nicht so besonders...",
                                          "Vielleicht ist dieser Smaragd radioaktiv...",
                                          "Ich fühle mich so schwach..."};
        } else {
            this.audio_path = "Audio/EN/";
            this.dialogue = new string[] {"Somehow, I don't feel very well...",
                                          "Maybe this emerald is radioactive..",
                                          "I feel so weak..."};
        }

        this.clips = new AudioClip[] {Resources.Load<AudioClip>(audio_path + "Emerald1"), 
                                      Resources.Load<AudioClip>(audio_path + "Emerald2"),
                                      Resources.Load<AudioClip>(audio_path + "Emerald3")};
            
        StartCoroutine(ShowDialogueAndDie());

    }

    private IEnumerator ShowDialogueAndDie() {
        yield return StartCoroutine(DialogueSystem.the().StartDialogueRoutine(dialogue, clips));
        StatusSystem.the().Eat(-10000);
        Destroy(this);
    }

    public override string GetItemName() {
        return "Emerald";
    }

    public override string GetItemDescription() {
        return "This is a Emerald.";
    }

    public override bool IsInteractible() {
        return true;
    }

    public override bool IsCollectible() {
        return false;
    }
}
