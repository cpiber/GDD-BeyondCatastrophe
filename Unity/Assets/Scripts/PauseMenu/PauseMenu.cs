using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool IsPaused = false;
    public GameObject PauseMenuObject;

    void Start()
    {
        
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) {

            if(IsPaused) {
                ResumeGame();
            } else {
                PauseGame();
            }
            IsPaused = !IsPaused;
        }
    }

    void PauseGame () {
        PauseMenuObject.SetActive(true);
        Time.timeScale = 0f;
    }

    void ResumeGame() {
        PauseMenuObject.SetActive(false);
        Time.timeScale = 1f;
    }
}
