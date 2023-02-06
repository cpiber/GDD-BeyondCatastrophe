using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

// This code was adapted from: https://www.youtube.com/watch?v=8oTYabhj248
public class DialogueSystem : GenericSingleton<DialogueSystem>
{
    [SerializeField] TextMeshProUGUI textComponent;
    [SerializeField] string[] lines = {"Hello this is a test", "Test line number 2", "Test Line number three"};
    [SerializeField] float speed = 0.1f;
    private int lineIndex;

    void Start(){
        gameObject.SetActive(false);
        // DialogueSystem.the().StartDialogue(new string[] {"Hello from Bedtime", "My god please work"});
        // StartDialogue(this.lines);
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
        StopAllCoroutines();
        if (textComponent.text == lines[lineIndex]){
            NextLine();
        } else {
            textComponent.text = lines[lineIndex];
        }
    }

    public void StartDialogue(string[] lines){
        gameObject.SetActive(true);
        textComponent.text = "";
        this.lines = lines;
        lineIndex = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine(){
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
            gameObject.SetActive(false);
        }
    }
}
