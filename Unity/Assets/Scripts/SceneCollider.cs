using UnityEngine;

public class SceneCollider : MonoBehaviour
{
    public string scene;

    void Start() {
        Destroy(this.GetComponent<SpriteRenderer>());
    }

    void OnTriggerEnter2D(Collider2D collider) {
        SceneLoader.the().LoadAScene(scene);
    }

    void OnTriggerExit2D(Collider2D collider) {
        SceneLoader.the().UnloadAScene(scene);
    }
}
