using UnityEngine;

public class PlayerController : MonoBehaviour
{
    void Update() {
        var move = Input.GetAxis("Horizontal");
        this.GetComponent<Rigidbody2D>().velocity = new Vector2(move * 5, 0);
    }

    void OnCollisionEnter2D(Collision2D collider) {
        var s = collider.gameObject.GetComponent<SceneObjectState>();
        Debug.Log(s);
        if (s) s.Destroy();
        else Destroy(collider.gameObject);
    }
}
