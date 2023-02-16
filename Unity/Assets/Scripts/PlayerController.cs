using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : GenericSingleton<PlayerController>
{
    const float INPUT_MIN = 0.225f;

    [SerializeField] [SceneProperty] string testScene;

    [SerializeField] Vector2 speed = new Vector2(5, 5);

    [SerializeField] InventoryManager inventory;

    private Item possibleCollectItem;
    public HeatedRoom CurrentRoom { get; set; }

    private Vector2 movement = Vector2.zero;
    [SerializeField] MovementRenderController renderController;
    public bool allowUserInteraction = true;

    private new Rigidbody2D rigidbody;

    void Start(){
        this.rigidbody = this.GetComponent<Rigidbody2D>();
        StartCoroutine(SetIndexNextFrame());
    }

    IEnumerator SetIndexNextFrame() {
        yield return null;
        var ui = InventoryUIManager.the();
        ui.SetSelectedSlot(ui.UseEquippedItemIndex);
    }

    void FixedUpdate() {
        if (DayNightSystem.the().IsPaused) return;
        MovePlayer();
    }

    void OnCollectItem() {
        if (DayNightSystem.the().IsPaused) return;
        if (!allowUserInteraction) return;
        if (possibleCollectItem != null && possibleCollectItem.IsCollectible()) {
            Debug.Assert(!possibleCollectItem.IsInteractible());
            // TODO add collision check and other collect features
            if (!inventory.AddBagItem(possibleCollectItem.name)) return;
            int sceneCount = SceneManager.sceneCount;
            for (int i = 0; i < sceneCount; i++)
            {
                if (SceneManager.GetSceneAt(i).path == testScene)
                    return;
            }
            if (possibleCollectItem.gameObject.TryGetComponent<SceneObjectState>(out var os)) os.Destroy();
            else Destroy(possibleCollectItem.gameObject);
            possibleCollectItem = null;
        }
    }

    void OnUseItem() {
        if (DayNightSystem.the().IsPaused) return;
        if (!allowUserInteraction) return;
        inventory.UseEquippedItem(InventoryUIManager.the().UseEquippedItemIndex);
    }

    // Set index of equipped item
    void OnSetIndexZero() {
        if (DayNightSystem.the().IsPaused) return;
        InventoryUIManager.the().SetSelectedSlot(0);
    }

    void OnSetIndexOne() {
        if (DayNightSystem.the().IsPaused) return;
        InventoryUIManager.the().SetSelectedSlot(1);
    }

    void OnSetIndexTwo() {
        if (DayNightSystem.the().IsPaused) return;
        InventoryUIManager.the().SetSelectedSlot(2);
    }

    void OnIncrementIndex() {
        if (DayNightSystem.the().IsPaused) return;
        var ui = InventoryUIManager.the();
        ui.SetSelectedSlot((ui.UseEquippedItemIndex + 1) % InventoryUIManager.MAX_EQUIPPED_ITEMS);
    }

    void OnDecrementIndex() {
        if (DayNightSystem.the().IsPaused) return;
        var ui = InventoryUIManager.the();
        ui.SetSelectedSlot(ui.UseEquippedItemIndex == 0 ? InventoryUIManager.MAX_EQUIPPED_ITEMS - 1 : ui.UseEquippedItemIndex - 1);
    }

    void OnInteractItem() {
        if (DayNightSystem.the().IsPaused) return;
        if (possibleCollectItem != null && possibleCollectItem.IsInteractible()) {
            Debug.Assert(!possibleCollectItem.IsCollectible());
            possibleCollectItem.UseItem();
        }
    }

    void OnOpenMenu() {
        if (DayNightSystem.the().IsPaused) return;
        inventory.UseBag();
    }


    void OnMove(InputValue mv) {
        if (InventoryUIManager.the().ShouldInhibitMovement) return;
        if (!allowUserInteraction) return;
        movement = mv.Get<Vector2>();
        if (Mathf.Abs(movement.x) < INPUT_MIN) movement.x = 0;
        if (Mathf.Abs(movement.y) < INPUT_MIN) movement.y = 0;
    }

    public void Stop() {
        movement = Vector2.zero;
    }

    public void SetPosition(Vector2 position) {
        transform.position = position;
        MovePlayer();
    }

    void MovePlayer(){
        rigidbody.velocity = movement * speed;
        var camera_position = new Vector3(transform.position.x, transform.position.y, -10);
        Camera.main.transform.position = camera_position;
        renderController.UpdateSprite(movement);
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        collider.gameObject.TryGetComponent<Item>(out possibleCollectItem);
    }

    private void OnTriggerExit2D(Collider2D collider) {
        if (possibleCollectItem != null && possibleCollectItem.gameObject == collider.gameObject) possibleCollectItem = null;
    }
}
