using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : GenericSingleton<PauseMenu>
{
    public bool IsPaused = false;
    public GameObject PauseMenuObject;
    [SerializeField] InputActionReference pauseMapAction;
    [SerializeField] RemapController remapController;


    public void TogglePause() {
        remapController.CloseMenu();
        if (IsPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
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
}
