using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeReference] static SceneLoader instance = null;

    private SerializableDictionary<string, int> scenedata = new SerializableDictionary<string, int>();

    public static SceneLoader the() {
        if (instance == null) instance = FindObjectOfType<SceneLoader>();
        return instance;
    }

    public void LoadAScene(string scene) {
        if (scenedata.ContainsKey(scene) && scenedata[scene] > 0) {
            scenedata[scene]++;
            Debug.Log($"Scene {scene} already loaded, only increasing count ({scenedata[scene]})");
            return;
        }
        if (!scenedata.ContainsKey(scene)) scenedata[scene] = 0;

#if UNITY_EDITOR
        for (int n = 0; n < SceneManager.sceneCount; ++n) {
            if (SceneManager.GetSceneAt(n).path == scene) {
                Debug.Log($"Scene {scene} not loaded but present, reusing that (index={n})");
                scenedata[scene]++;
                return;
            }
        }
#endif

        var loader = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
        Debug.Assert(loader != null);
        loader.completed += (op) => {
            Debug.Log($"Loaded scene {scene}");
            scenedata[scene]++;
        };
    }

    public void UnloadAScene(string scene) {
        scenedata[scene]--;
        if (scenedata[scene] > 0) {
            Debug.Log($"Scene {scene} still being loaded ({scenedata[scene]})");
            return;
        }

        scenedata[scene] = 0;
        SceneManager.UnloadSceneAsync(scene).completed += (op) => {
            Debug.Log($"Unloaded scene {scene}");
            Resources.UnloadUnusedAssets();
        };
    }
}
