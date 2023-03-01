using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class RemapController : MonoBehaviour
{
    [SerializeField] List<InputActionReference> triggerActions;

    [SerializeField] List<TextMeshProUGUI> texts;

    InputActionRebindingExtensions.RebindingOperation rebindOperation;

    private bool IsOpen = false;

    [SerializeField] InputActionReference pauseAction;


    void Start() {
        int index = 0;
        foreach (var action in triggerActions) {
            texts[index].text = triggerActions[index].action.bindings[0].effectivePath.Split('/')[1].ToUpper();
            var name = triggerActions[index].action.name.Contains("OpenPauseMenu");
            index++;
        }
       
    }

    public void ToggleMenu() {
        if (IsOpen == false) {
            gameObject.SetActive(true);
            DeactivatePauseKey();
        } else {
            gameObject.SetActive(false);
            ActivatePauseKey();
        }
        IsOpen = !IsOpen;
    }

    public void CloseMenu() {
        gameObject.SetActive(false);
        ActivatePauseKey();
        IsOpen = false;
    }


    void OnDestroy() {
        int index = 0;
        foreach (var action in triggerActions) {
            var path = triggerActions[index].action.bindings[0].path;
            triggerActions[index].action.ApplyBindingOverride(0, path);
            index++;
        }
    }
        
    public void RemapKey(int index) {
        texts[index].text = "<press any key>";
        triggerActions[index].action.Disable();
        rebindOperation = triggerActions[index].action.PerformInteractiveRebinding().WithControlsExcluding("Mouse").OnComplete(triggerOperation => FinishedRebinding(index)).Start();
    }

    public void FinishedRebinding(int index) {
        InputAction act = triggerActions[index].action;
        texts[index].text = triggerActions[index].action.bindings[0].effectivePath.Split('/')[1].ToUpper();
        triggerActions[index].action.Enable();
        rebindOperation.Dispose();
        rebindOperation.Reset();
    }

    public void DeactivatePauseKey() {
        pauseAction.action.Disable();
    }

    public void ActivatePauseKey() {
        pauseAction.action.Enable();
    }
}
