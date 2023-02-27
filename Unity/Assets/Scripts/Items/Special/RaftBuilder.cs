using UnityEngine;

public class RaftBuilder : PermanentItem
{
    [SerializeField] string globalKey = "RaftBuilder";
    [SerializeField] GameObject raft;

    void Start() {
        var state = GlobalSceneState.the().getState(globalKey);
        if (state != null && !state.exists) Destroy(this);
        SetSprite();
    }

    public override void UseItem() {
        raft.SetActive(true);
        GlobalSceneState.the().setExists(globalKey, false);
        PlayerController.the().UnregisterCollectItem(this);
        PlayerController.the().RegisterCollectItem(raft.GetComponent<Item>());
        Destroy(this);
    }

    public override string GetItemName() {
        return "Raft Builder";
    }

    public override string GetItemDescription() {
        return "";
    }

    public override bool IsInteractible() {
        return true;
    }

    public override bool IsCollectible() {
        return false;
    }
}
