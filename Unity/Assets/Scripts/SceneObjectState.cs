using UnityEngine;

public class SceneObjectState : MonoBehaviour
{
    [SerializeField] string globalKey;
    
    void Start() {
        var state = GlobalSceneState.the().getState(globalKey, transform.position);
        if (!state.exists) Destroy(gameObject);
        else transform.position = state.position;
    }

    void OnDestroy() {
        GlobalSceneState.the().setPosition(globalKey, transform.position);
        Debug.Log($"Destorying with update position {globalKey}");
    }

    public void Destroy() {
        GlobalSceneState.the().setExists(globalKey, false);
        Debug.Log($"Destorying completely {globalKey}");
        Destroy(gameObject);
    }
}
