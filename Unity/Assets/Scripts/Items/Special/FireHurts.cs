using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FireHurts : MonoBehaviour
{
    string[] dialogue = null;
    AudioClip[] clips = null;
    string audio_path = null;
    public const string FIRE_HURTS = "fireHurts";
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        StatusSystem.the().updateHealth(-10);

        if (!ProgressSystem.the().getProgress(FIRE_HURTS))
        {
            Debug.Log("IN dialogFirstUse");
            // TODO: check for language flag
            if(false){
                audio_path = "Audio/DE/";
                dialogue = new string[] {"Ahh, das tut weh - Ich sollte nicht zu nahe ans Feuer gehen!"
                };
            } else {
                audio_path = "Audio/EN/";
                dialogue = new string[] {"Ahh, that hurts - I shouldn't go to close to the fire!"};
            }

            clips = new AudioClip[] { Resources.Load<AudioClip>(audio_path + "FireHurts1") };
            DialogueSystem.the().StartDialogue(dialogue, clips);
            ProgressSystem.the().setProgress(FIRE_HURTS);   
        }

    }
    
}
