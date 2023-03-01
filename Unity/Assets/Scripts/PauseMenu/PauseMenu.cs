using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenu : GenericSingleton<PauseMenu>
{
    public bool IsPaused = false;
    public GameObject PauseMenuObject;
    [SerializeField] InputActionReference pauseMapAction;
    [SerializeField] RemapController remapController;
    [SerializeField] PlayerInput playerInput;
    [SerializeField] GameObject rebindButton;


    public void TogglePause() {
        remapController.CloseMenu();
        if (IsPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
            DisableRebindIfNotKeyboard();
            StartCoroutine(SetSelected());
        }

        IsPaused = !IsPaused;
    }

    void PauseGame()
    {
        PauseMenuObject.SetActive(true);
        Time.timeScale = 0f;
        PlayerController.the().allowUserInteraction = false;
    }

    void ResumeGame()
    {
        PauseMenuObject.SetActive(false);
        Time.timeScale = 1f;
        PlayerController.the().allowUserInteraction = true;
    }

    public void TogglePauseWithoutTimeChange() {
        if (IsPaused)
        {
            PauseMenuObject.SetActive(false);
        }
        else
        {
            PauseMenuObject.SetActive(true);
        }
        PlayerController.the().allowUserInteraction = IsPaused;
        IsPaused = !IsPaused;
    }

    private IEnumerator SetSelected() {
        yield return null;
        var obj = GetComponentInChildren<Slider>().gameObject;
        Debug.Log($"Selecting {obj}");
        EventSystem.current.SetSelectedGameObject(obj);
    }

    private void DisableRebindIfNotKeyboard() {
        rebindButton.SetActive(playerInput.currentControlScheme == InventoryUIManager.KEYBOARD_SCHEME);
    }
}
