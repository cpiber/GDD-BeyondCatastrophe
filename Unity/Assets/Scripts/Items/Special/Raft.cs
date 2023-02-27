using System.Collections;
using UnityEngine;

public class Raft : PermanentItem
{
    [SerializeField] Transform[] wayPoints;
    [SerializeField] Transform[] endPoints = new Transform[2];
    [SerializeField] float speed = 5;
    private bool reverse = false;

    public override void UseItem() {
        var player = PlayerController.the();
        player.allowUserInteraction = false;
        player.Stop();
        player.SetPosition(transform.position);
        StartCoroutine(followPath());
    }

    IEnumerator followPath() {
        Debug.Log($"Start path. reverse? {reverse}");
        var currentWaypoint = !reverse ? 0 : wayPoints.Length - 1;
        var increment = !reverse ? 1 : -1;
        Debug.Log($"Next waypoint: {currentWaypoint} (increment {increment})");
        var render = GetComponent<MovementRenderController>();
        var player = PlayerController.the();
        var playerRender = player.GetComponent<MovementRenderController>();
        var scale = player.transform.localScale;
        player.transform.localScale /= 1.5f;

        while (currentWaypoint >= 0 && currentWaypoint < wayPoints.Length) {
            yield return new WaitForFixedUpdate();
            var newPos = Vector3.MoveTowards(transform.position, wayPoints[currentWaypoint].position, speed);
            var direction = wayPoints[currentWaypoint].position - transform.position;
            var dir = new Vector2(direction.x, direction.y);
            transform.position = newPos;
            player.SetPosition(newPos);
            render.UpdateSprite(dir);
            playerRender.UpdateSprite(dir);

            if (newPos == wayPoints[currentWaypoint].position) {
                currentWaypoint += increment;
                Debug.Log($"Next waypoint: {currentWaypoint}");
            }
        }

        var end = endPoints[reverse ? 0 : 1];
        player.SetPosition(end.position);
        player.allowUserInteraction = true;
        player.transform.localScale = scale;
        reverse = !reverse;
        Debug.Log($"End at waypoint {currentWaypoint}, now reverse? {reverse}");
    }

    public override string GetItemName() {
        return "Raft";
    }

    public override string GetItemDescription() {
        return "No need to get your feet wet, use the raft to cross the water.";
    }

    public override bool IsInteractible() {
        return true;
    }

    public override bool IsCollectible() {
        return false;
    }
}
