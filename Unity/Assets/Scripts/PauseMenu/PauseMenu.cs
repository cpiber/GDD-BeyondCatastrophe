using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public static bool IsPaused = false;

    public GameObject PauseMenuObject;

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
    }

    void ResumeGame()
    {
        PauseMenuObject.SetActive(false);
        Time.timeScale = 1f;
    }
}
