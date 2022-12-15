using UnityEngine;

public class SceneCollider : MonoBehaviour
{
    [SerializeField] [SceneProperty] string scene;
    public GameObject unloadObject;

    void Start() {
        Destroy(this.GetComponent<SpriteRenderer>());
        if (unloadObject != null)
        {
            unloadObject.SetActive(true);  
        }
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if(collider.tag == "Player"){
            SceneLoader.the().LoadAScene(scene);
            if (unloadObject != null)
            {
                unloadObject.SetActive(false);  
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider) {
        if(collider.tag == "Player"){
            SceneLoader.the().UnloadAScene(scene);
            if (unloadObject != null)
            {
                unloadObject.SetActive(true); 
            }
        }
    }
}
