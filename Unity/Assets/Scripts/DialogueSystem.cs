using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

// This code was adapted from: https://www.youtube.com/watch?v=8oTYabhj248
public class DialogueSystem : GenericSingleton<DialogueSystem>
{
    [SerializeField] GameObject dialogueBox;
    [SerializeField] TextMeshProUGUI textComponent;
    [SerializeField] string[] lines = {"Hello this is a test", "Test line number 2", "Test Line number three"};
    [SerializeField] float speed = 0.1f;
    private int lineIndex;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] clips = {};
    [SerializeField] float volume = 1f;

    public bool IsOpen => dialogueBox.activeSelf;
  
    void Start(){
        dialogueBox.SetActive(false);
        /* 
        // -> Call Dialogue system like this
        this.clips = new AudioClip[] {Resources.Load<AudioClip>("Audio/test1"), 
                                      Resources.Load<AudioClip>("Audio/test1"),
                                      Resources.Load<AudioClip>("Audio/test1")};
        // DialogueSystem.the().StartDialogue(new string[] {"Hello from Bedtime", "My god please work"});
        StartDialogue(this.lines, this.clips);
        */
    }

    void OnClick (InputValue input) {
        // LeftClick
        if (input.Get<float>() > 0.5){
            CompleteDialogue();
        }
    }

    void OnSubmit () {
        // Enter
        CompleteDialogue();
    }


    void CompleteDialogue() {
        if (!dialogueBox.activeSelf) return;
        StopAllCoroutines();
        if (textComponent.text == lines[lineIndex]){
            NextLine();
        } else {
            textComponent.text = lines[lineIndex];
        }
    }

    [ContextMenu("Start Dialogue")]
    public void StartDialogueAsIs() {
        StartDialogue(this.lines, this.clips);
    }
    public void StartDialogue(string[] lines, AudioClip[] clips){
        dialogueBox.SetActive(true);
        InventoryUIManager.the().CloseAllUI();
        textComponent.text = "";
        this.lines = lines;
        this.clips = clips;
        lineIndex = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine(){
        // TODO: only play audio if flag set in menue
        audioSource.Stop();
        audioSource.PlayOneShot(lineIndex < clips.Length ? clips[lineIndex] : null, volume);
        for (int i = 0; i < lines[lineIndex].Length; i++){
            textComponent.text += lines[lineIndex][i];
            yield return new WaitForSeconds(speed);
        }
    }

    void NextLine(){
        if (lineIndex < lines.Length - 1){
            lineIndex++;
            textComponent.text = "";
            StartCoroutine(TypeLine());
        } else {
            dialogueBox.SetActive(false);
            // TODO: only stop if audio flag set
            audioSource.Stop();
        }
    }
}
