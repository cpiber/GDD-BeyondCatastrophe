using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public static bool IsPaused = false;

    public GameObject PauseMenuObject;

    [SerializeField] InputActionReference pauseMapAction;


    //public InputAction pauseMenu;

    void Start()
    {
    }

    /*void Update()
    {
        if(pauseMenu.readValue())
        {
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
    }*/

    public void TogglePause() {
        if (IsPaused)
        {
            ResumeGame();
            ActivateInventoryKey();
        }
        else
        {
            PauseGame();
            DeactivateInventoryKey();
        }

        IsPaused = !IsPaused;
    }

    void PauseGame()
    {
        PauseMenuObject.SetActive(true);
        Time.timeScale = 0f;
    }

    void ResumeGame()
    {
        PauseMenuObject.SetActive(false);
        Time.timeScale = 1f;
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
        IsPaused = !IsPaused;
    }

    public void DeactivateInventoryKey() {
        pauseMapAction.action.Disable();
    }

    public void ActivateInventoryKey() {
        pauseMapAction.action.Enable();
    }
}
