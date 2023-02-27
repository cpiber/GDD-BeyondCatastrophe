using UnityEngine;

public class FollowPlayer : GenericSingleton<FollowPlayer>
{
    public const string CHILD_FOLLOWING = "childCollected";

    [SerializeField] [HideInInspector] PlayerController player;
    [SerializeField] [HideInInspector] MovementRenderController render;
    [SerializeField] [HideInInspector] new Rigidbody2D rigidbody;
    [SerializeField] float speed = 5;
    [SerializeField] float minDist = 10;

    void Start() {
        player = PlayerController.the();
        render = GetComponent<MovementRenderController>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        var newPos = Vector3.MoveTowards(transform.position, player.transform.position, speed);
        var direction = player.transform.position - transform.position;
        var dir = new Vector2(direction.x, direction.y);
        if (dir.sqrMagnitude < minDist) dir = Vector2.zero;
        rigidbody.velocity = dir;
        render.UpdateSprite(dir);
    }
}
