using UnityEngine;

public class SceneCollider : MonoBehaviour
{
    [SerializeField] [SceneProperty] string scene;
    public GameObject unloadObject;

    void Start() {
        Destroy(this.GetComponent<SpriteRenderer>());
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if(collider.tag == "Player"){
            SceneLoader.the().LoadAScene(scene);
            unloadObject.SetActive(false);
        }
    }

    void OnTriggerExit2D(Collider2D collider) {
        if(collider.tag == "Player"){
            SceneLoader.the().UnloadAScene(scene);
            unloadObject.SetActive(true);
        }
    }
}
