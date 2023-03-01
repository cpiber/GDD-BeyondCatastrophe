using System.Collections;
using UnityEngine;

public class DaddyGreeter : PermanentItem
{
    [SerializeField] [HideInInspector] new Rigidbody2D rigidbody;

    private string[] dialogue = null;
    private AudioClip[] clips = null;

    private string audio_path = null;

    public override void UseItem () {
        
        if(!DialogueSystem.the().english){
            this.audio_path = "Audio/DE/";
            this.dialogue = new string[] {"(Kind) Papa! Du hast mich gefunden!",
                                          "...",
                                          "(Spieler) Wir sollten von hier verschwinden, irgendetwas kommt.",
                                          "Ich habe einen Hafen weiter unten gesehen, vielleicht kommen wir so von der Insel weg."};
        } else {
            this.audio_path = "Audio/EN/";
            this.dialogue = new string[] {"(Child) Daddy! You found me!",
                                          "...",
                                          "(You) We should leave this island. Something is coming.",
                                          "I saw a port a bit below, maybe we can find something to escape there."};
        }

        this.clips = new AudioClip[] {Resources.Load<AudioClip>(audio_path + "DaddyGreeter1"),
                                      Resources.Load<AudioClip>(audio_path + "DaddyGreeter2"),
                                      Resources.Load<AudioClip>(audio_path + "DaddyGreeter3"), 
                                      Resources.Load<AudioClip>(audio_path + "DaddyGreeter4")};
    
        StartCoroutine(ShowDialogueAndDestory());
    }

    private IEnumerator ShowDialogueAndDestory() {
        yield return StartCoroutine(DialogueSystem.the().StartDialogueRoutine(dialogue, clips));
        ProgressSystem.the().setProgress(FollowPlayer.CHILD_FOLLOWING);
        gameObject.GetComponent<FollowPlayer>().enabled = true;
        PlayerController.the().UnregisterCollectItem(this);
        Destroy(this);
    }

    void FixedUpdate() {
        if (rigidbody == null) rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = Vector2.zero;
    }

    public override string GetItemName() {
        return "Child";
    }

    public override string GetItemDescription() {
        return "Hello from your child.";
    }

    public override bool IsInteractible() {
        return true;
    }

    public override bool IsCollectible() {
        return false;
    }
}
