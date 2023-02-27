using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public class PlayerController : GenericSingleton<PlayerController>
{
    const float INPUT_MIN = 0.225f;

    [SerializeField] [SceneProperty] string testScene;

    [SerializeField] Vector2 speed = new Vector2(5, 5);

    [SerializeField] InventoryManager inventory;

    private Item possibleCollectItem;
    [SerializeField] [HideInInspector] List<Item> possibleCollectItems = new List<Item>();
    [SerializeField] Color itemOutlineColor;
    [SerializeField] float itemOutlineAdd = .1f;
    public Color ItemOutlineColor => itemOutlineColor;
    public HeatedRoom CurrentRoom { get; set; }

    private Vector2 movement = Vector2.zero;
    [SerializeField] MovementRenderController renderController;
    public bool allowUserInteraction = true;

    private new Rigidbody2D rigidbody;
    [SerializeField] private float timeSinceIdle = 0;
    public float TimeSinceIdle => timeSinceIdle;


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
        if (rigidbody.velocity.SqrMagnitude() > float.Epsilon) timeSinceIdle = 0;
        else timeSinceIdle += Time.deltaTime;
    }

    void OnCollectItem() {
        if (DayNightSystem.the().IsPaused) return;
        if (!allowUserInteraction) return;
        if (possibleCollectItem != null && possibleCollectItem.IsCollectible()) {
            Debug.Assert(!possibleCollectItem.IsInteractible());
            // TODO add collision check and other collect features
            if (!inventory.AddBagItem(possibleCollectItem)) return;
            int sceneCount = SceneManager.sceneCount;
            for (int i = 0; i < sceneCount; i++)
            {
                if (SceneManager.GetSceneAt(i).path == testScene)
                    return;
            }
            if (possibleCollectItem.gameObject.TryGetComponent<SceneObjectState>(out var os)) os.Destroy();
            else Destroy(possibleCollectItem.gameObject);
            // NOTE: destroying will trigger OnTriggerExit, which will reset the selected item
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
        if (!allowUserInteraction) return;
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

    public Vector2 GetPosition()
    {
        return transform.position;
    }

    public InventoryManager GetInventory()
    {
        return inventory;
    }
    
    void MovePlayer(){
        rigidbody.velocity = movement * speed;
        var camera_position = new Vector3(transform.position.x, transform.position.y, -10);
        Camera.main.transform.position = camera_position;
        renderController.UpdateSprite(movement);
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        collider.gameObject.TryGetComponent<Item>(out var item);
        if (item != null && (item.IsCollectible() || item.IsInteractible())) {
            if (possibleCollectItem != null) UnhighlightItem(possibleCollectItem);
            possibleCollectItems.Add(item);
            possibleCollectItem = item;
            Debug.Log($"Updated collect item to {item.name}. Maintaining {possibleCollectItems.Count} items total.");
            HighlightItem(item);
        }
    }

    private void OnTriggerExit2D(Collider2D collider) {
        if (collider.gameObject.TryGetComponent<Item>(out var item)) {
            possibleCollectItems.Remove(item);
            Debug.Log($"Removed item {item.name}. Maintaining {possibleCollectItems.Count} items total.");
            UnhighlightItem(item);
        }
        if (possibleCollectItem != null && possibleCollectItem.gameObject == collider.gameObject) {
            possibleCollectItem = null;
            UnhighlightItem(item);
            SelectNextCollectItem();
        }
    }

    public void UnregisterCollectItem(Item item) {
        Debug.Log($"Removing possible select item {item} due to request");
        possibleCollectItems.Remove(item);
        if (possibleCollectItem == item) SelectNextCollectItem();
    }

    public void RegisterCollectItem(Item item) {
        Debug.Log($"Adding possible select item {item} due to request");
        possibleCollectItems.Add(item);
        if (possibleCollectItem == null) possibleCollectItem = item;
    }

    private void HighlightItem(Item item) {
        var renderer = item.GetComponent<Renderer>();
        if (renderer.material.shader.name == "Shader Graphs/GlowShader" && (item.IsCollectible() || item.IsInteractible())) {
            renderer.material.SetColor("_OutlineColor", itemOutlineColor);
            renderer.material.SetFloat("_OutlineSize", itemOutlineAdd);
        }
        if (item.OutlineObject != null && (item.IsCollectible() || item.IsInteractible())) {
            item.OutlineObject.SetActive(true);
        }
    }

    private void UnhighlightItem(Item item) {
        var renderer = item.GetComponent<Renderer>();
        if (renderer.material.shader.name == "Shader Graphs/GlowShader" && (item.IsCollectible() || item.IsInteractible())) {
            renderer.material.SetColor("_OutlineColor", Color.white);
            renderer.material.SetFloat("_OutlineSize", 0);
        }
        if (item.OutlineObject != null) {
            item.OutlineObject.SetActive(false);
        }
    }

    private void SelectNextCollectItem() {
        if (possibleCollectItem != null) UnhighlightItem(possibleCollectItem);
        possibleCollectItem = possibleCollectItems.Count > 0 ? possibleCollectItems[0] : null;
        if (possibleCollectItem != null) HighlightItem(possibleCollectItem);
        if (possibleCollectItem == null) possibleCollectItem = null; // Unity magic
        Debug.Log($"Updated collect item to {possibleCollectItem?.name ?? "null"} via select. Maintaining {possibleCollectItems.Count} items total.");
    } 
}
