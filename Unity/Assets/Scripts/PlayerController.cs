using DefaultNamespace;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    void Update()
    {
        var move_x = Input.GetAxis("Horizontal");
        var move_y = Input.GetAxis("Vertical");
        this.GetComponent<Rigidbody2D>().velocity = new Vector2(move_x * 5, move_y * 5);
    }

    void OnCollisionEnter2D(Collision2D collider) {
        var s = collider.gameObject.GetComponent<SceneObjectState>();
        var border = collider.gameObject.GetComponent<Border>();
        if (border)
        {
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
        }
        else if (s) s.Destroy();
        else Destroy(collider.gameObject);
        
    }
}
