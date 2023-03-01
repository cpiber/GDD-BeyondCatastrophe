using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Threading.Tasks;
using UnityEngine.Events;
using System;

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
    [SerializeField] private UnityEvent dialogueDone = new UnityEvent();
    
    public bool IsOpen => dialogueBox.activeSelf;
    private bool blockDialogue = false;
    private bool blockAudio = false;

  
    void Start(){
        dialogueBox.SetActive(false);
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

    [ContextMenu("Cancel Dialogue")]
    public void CancelDialogue() {
        StopAllCoroutines();
        dialogueBox.SetActive(false);
        audioSource.Stop();
        dialogueDone.Invoke();
    }

    [ContextMenu("Start Dialogue")]
    public void StartDialogueAsIs() {
        StartDialogue(this.lines, this.clips);
    }
    public void StartDialogue(string[] lines, AudioClip[] clips){
        dialogueDone.Invoke(); // kill previous invocations
        StopAllCoroutines();
        InventoryUIManager.the().CloseAllUI();
        textComponent.text = "";
        this.lines = lines;
        this.clips = clips;
        lineIndex = 0;
        StartCoroutine(TypeLine());
    }
    public IEnumerator StartDialogueRoutine(string[] lines, AudioClip[] clips){
        StartDialogue(lines, clips);
        // wait for event, based on https://stackoverflow.com/a/51372785/
        var trigger = false;
        Action action = () => trigger = true;
        dialogueDone.AddListener(action.Invoke);
        yield return new WaitUntil(() => trigger);
        dialogueDone.RemoveListener(action.Invoke);
    }

    void PlayAudio() {
        audioSource.Stop();
        audioSource.PlayOneShot(lineIndex < clips.Length ? clips[lineIndex] : null, volume);
    }

    IEnumerator TypeLine(){
        if (!blockAudio) {
            PlayAudio();
        } 
        if (!blockDialogue) {
            dialogueBox.SetActive(true);

            for (int i = 0; i < lines[lineIndex].Length; i++){
                textComponent.text += lines[lineIndex][i];
                yield return new WaitForSeconds(speed);
            }
        }
    }


    void NextLine(){
        if (lineIndex < lines.Length - 1){
            lineIndex++;
            textComponent.text = "";
            StartCoroutine(TypeLine());
        } else {
            dialogueBox.SetActive(false);
            audioSource.Stop();
            dialogueDone.Invoke();
        }
    }

    public void SetBlockDialogue() {
        blockDialogue = !blockDialogue;
    }
    public void SetBlockAudio() {
        blockAudio = !blockAudio;
    }

    public void SetVolume(System.Single newVolume) {
        volume = newVolume;
    }
}
