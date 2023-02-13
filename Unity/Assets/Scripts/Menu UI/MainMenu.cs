using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject quitButton;

    void Start() {
#if UNITY_WEBGL || UNITY_EDITOR
        quitButton.SetActive(false);
#endif
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("MainGameScene");
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Debug.Log("Quiting game...");
        Application.Quit();
    }
}
