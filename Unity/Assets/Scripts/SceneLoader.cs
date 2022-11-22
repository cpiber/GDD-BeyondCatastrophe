using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader
{
    static SceneLoader instance = null;
    private SceneLoader() {}

    private Dictionary<string, int> scenedata = new Dictionary<string, int>();

    public static SceneLoader the() {
        if (instance == null) instance = new SceneLoader();
        return instance;
    }

    public void LoadAScene(string scene) {
        if (scenedata.ContainsKey(scene) && scenedata[scene] > 0) {
            Debug.Log($"Scene {scene} already loaded, only increasing count");
            scenedata[scene]++;
            return;
        }
        if (!scenedata.ContainsKey(scene)) scenedata[scene] = 0;

        var loader = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
        Debug.Assert(loader != null);
        loader.completed += (op) => {
            Debug.Log($"Loaded scene {scene}");
            scenedata[scene]++;
        };
    }

    public void UnloadAScene(string scene) {
        Debug.Assert(scenedata.ContainsKey(scene));
        scenedata[scene]--;
        if (scenedata[scene] > 0) {
            Debug.Log($"Scene {scene} still being loaded");
            return;
        }

        SceneManager.UnloadSceneAsync(scene).completed += (op) => {
            Debug.Log($"Unloaded scene {scene}");
            Resources.UnloadUnusedAssets();
        };
    }
}
