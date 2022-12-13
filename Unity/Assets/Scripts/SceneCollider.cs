using UnityEngine;

public class SceneCollider : MonoBehaviour
{
    [SerializeField] [SceneProperty] string scene;

    void Start() {
        Destroy(this.GetComponent<SpriteRenderer>());
    }

    void OnTriggerEnter2D(Collider2D collider) {

        if(collider.tag == "Player"){
            SceneLoader.the().LoadAScene(scene);
        }
    }

    void OnTriggerExit2D(Collider2D collider) {
        if(collider.tag == "Player"){
            SceneLoader.the().UnloadAScene(scene);
        }
    }
}
