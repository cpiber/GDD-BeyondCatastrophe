using UnityEngine;

public class SceneObjectState : MonoBehaviour
{
    [SerializeField] string globalKey;
    
    void Start() {
        Debug.Assert(globalKey != "", $"You need to fill the globalKey property uniquely ({name})", this.gameObject);
        var state = GlobalSceneState.the().getState(globalKey);
        if (state == null) return;
        if (!state.exists) Destroy(gameObject);
        else transform.position = state.position;
        Debug.Log($"Loading {globalKey}: exists? {state.exists}");
    }

    void OnDestroy() {
        GlobalSceneState.the()?.setPosition(globalKey, transform.position);
        Debug.Log($"Destorying with update position {globalKey}");
    }

    public void Destroy() {
        GlobalSceneState.the().setExists(globalKey, false);
        Debug.Log($"Destorying completely {globalKey}");
        Destroy(gameObject);
    }
}
