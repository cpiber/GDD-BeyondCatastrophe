using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject quitButton;
    [SerializeField] [SceneProperty] string mainScene;
    [SerializeField] List<string> pointsToFocus;
    [SerializeField] int daysPerPoint;
    [SerializeField] float cameraSpeed;

    void Start() {
#if UNITY_WEBGL || UNITY_EDITOR
        quitButton.SetActive(false);
#endif
        if (string.IsNullOrEmpty(mainScene)) return;
        SceneManager.LoadSceneAsync(mainScene, LoadSceneMode.Additive).completed += (op) => {
            Destroy(GameObject.FindObjectOfType<PlayerController>().gameObject);
            Destroy(GameObject.FindObjectOfType<FancyHud>().gameObject);
            foreach (var obj in SceneManager.GetSceneByPath(mainScene).GetRootGameObjects()) {
                if (obj.name == "EventSystem" || obj.name == "UI" || obj.name == "Inventory") Destroy(obj);
                else if (obj.name == "Main Camera") {
                    obj.GetComponent<Camera>().enabled = false;
                    obj.GetComponent<AudioListener>().enabled = false;
                }
            }
            GameObject.FindObjectOfType<DayNightSystem>().SetCutscene();
            if (pointsToFocus.Count > 0) FocusPoint(pointsToFocus[0]);
            if (pointsToFocus.Count > 1) StartCoroutine(IterateMainPoints());
        };
    }

    private Vector3 FocusPoint(string name, bool setcam = true) {
        var wrapper = GameObject.Find("MainMenuPoints");
        for (var i = 0; i < wrapper.transform.childCount; i ++) {
            var obj = wrapper.transform.GetChild(i).gameObject;
            if (obj.name != name) continue;
            obj.SetActive(true);
            var camera_position = new Vector3(obj.transform.position.x, obj.transform.position.y, -10);
            if (!setcam) return camera_position;
            Camera.main.transform.position = camera_position;
            return camera_position;
        }
        Debug.LogWarning($"Could not load point {name}");
        return Vector3.zero;
    }

    private void UnFocusPoint(string name) {
        var wrapper = GameObject.Find("MainMenuPoints");
        for (var i = 0; i < wrapper.transform.childCount; i ++) {
            var obj = wrapper.transform.GetChild(i).gameObject;
            if (obj.name != name) continue;
            obj.SetActive(false);
            return;
        }
        Debug.LogWarning($"Could not load point {name}");
    }

    private IEnumerator IterateMainPoints() {
        var curpoint = 0;
        while (true) {
            for (var i = 0; i < daysPerPoint; i++) {
                yield return new WaitForSeconds(1.1f);
                yield return new WaitWhile(() => DayNightSystem.the().TimeInDay >= 1);
                Debug.Log($"Day {i} ({DayNightSystem.the().TimeInDay})");
            }
            var oldpoint = curpoint;
            curpoint = (curpoint + 1) % pointsToFocus.Count;
            var pos = FocusPoint(pointsToFocus[curpoint], false);
            var speed = Vector3.Distance(pos, Camera.main.transform.position) / 100 * cameraSpeed;
            while (Camera.main.transform.position != pos) {
                yield return new WaitForFixedUpdate();
                Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, pos, speed);
            }
            UnFocusPoint(pointsToFocus[oldpoint]);
        }
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

    void OnDestroy() {
        StopAllCoroutines();
    }
}
