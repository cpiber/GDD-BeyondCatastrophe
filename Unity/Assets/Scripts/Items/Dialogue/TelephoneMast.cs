using UnityEngine;

public class TelephoneMast : PermanentItem
{
    private string[] dialogue = null;
    private AudioClip[] clips = null;

    private string audio_path = null;
    public override void UseItem () {

        // TODO: check for language flag
        if(false){
            this.audio_path = "Audio/DE/";
            this.dialogue = new string[] {"ring ring ring... Keiner hebt ab."};
        } else {
            this.audio_path = "Audio/EN/";
            this.dialogue = new string[] {"ring ring ring... No one picks up."};
        }

        this.clips = new AudioClip[] {Resources.Load<AudioClip>(audio_path + "Telephonemast1")};
            
        DialogueSystem.the().StartDialogue(dialogue, clips);
    }

    public override string GetItemName() {
        return "Telephone Mast";
    }

    public override string GetItemDescription() {
        return "";
    }

    public override bool IsInteractible() {
        return true;
    }

    public override bool IsCollectible() {
        return false;
    }
}
