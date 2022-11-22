using UnityEngine;

public class PlayerController : MonoBehaviour
{
    void Update() {
        var move = Input.GetAxis("Horizontal");
        this.GetComponent<Rigidbody2D>().velocity = new Vector2(move * 5, 0);
    }
}
