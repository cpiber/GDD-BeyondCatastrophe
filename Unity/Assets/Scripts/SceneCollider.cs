using UnityEngine;

public class SceneCollider : MonoBehaviour
{
    public string scene;

    void Start() {
        Destroy(this.GetComponent<SpriteRenderer>());
    }

    void OnTriggerEnter2D(Collider2D collider) {
        Debug.Log($"Collison with {collider}");
        SceneLoader.the().LoadAScene(scene);
    }

    void OnTriggerExit2D(Collider2D collider) {
        Debug.Log($"Collison with {collider}");
        SceneLoader.the().UnloadAScene(scene);
    }
}
