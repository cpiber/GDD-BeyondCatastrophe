using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Vector2 speed = new Vector2(5, 5);
    void Update()
    {
        MovePlayer();
    }

    void MovePlayer(){
        var move_x = Input.GetAxis("Horizontal");
        var move_y = Input.GetAxis("Vertical");
        this.GetComponent<Rigidbody2D>().velocity = new Vector2(move_x * speed.x, move_y * speed.y);
        var camera_position = new Vector3(transform.position.x, transform.position.y, -10);
        Camera.main.transform.position = camera_position;
    }

    void OnCollisionEnter2D(Collision2D collider) {
        var s = collider.gameObject.GetComponent<SceneObjectState>();
        var border = collider.gameObject.GetComponent<Border>();
        if (border){
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
        }
        else if (s) s.Destroy();
        else Destroy(collider.gameObject);
        
    }
}
