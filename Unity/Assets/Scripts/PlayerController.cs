using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] [SceneProperty] string testScene;

    [Serializable]
    public struct Sprites {
        public Sprite left;
        public Sprite right;
        public Sprite front;
        public Sprite back;
    }

    [SerializeField] int characterIndex = 0;

    private SpriteRenderer spriteRenderer;

    [SerializeField] Sprites[] spriteList;

    [SerializeField] Vector2 speed = new Vector2(5, 5);

    [SerializeField] InventoryManager inventory;

    private Item possibleCollectItem;

    void Start(){
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update(){
        MovePlayer();
        UseItem();
        InteractItem();
        CollectItem();
        Inventory();
    }

    void CollectItem() {
        if (Input.GetButtonDown("CollectItem") && possibleCollectItem != null && possibleCollectItem.IsCollectible()) {
            // TODO add collision check and other collect features
            inventory.AddBagItem(possibleCollectItem.name);
            int sceneCount = SceneManager.sceneCount;
            for (int i = 0; i < sceneCount; i++)
            {
                if (SceneManager.GetSceneAt(i).path == testScene)
                    return;
            }
            possibleCollectItem.gameObject.SetActive(false);
            possibleCollectItem = null;
        }
    }

    void UseItem() {
        if (Input.GetButtonDown("UseItem")) {
            inventory.UseEquippedItem(0);
        }
    }

    void InteractItem() {
        if (Input.GetButtonDown("Interact") && possibleCollectItem != null && possibleCollectItem.IsInteractible()) {
            possibleCollectItem.UseItem();
        }
    }

    void Inventory() {
        if (Input.GetButtonDown("OpenMenu")) {
            inventory.UseBag();
        }
    }

    void MovePlayer(){
        var move_x = Input.GetAxis("Horizontal");
        var move_y = Input.GetAxis("Vertical");
        var move_vec = new Vector2(move_x, move_y);
        this.GetComponent<Rigidbody2D>().velocity = move_vec * speed;
        var camera_position = new Vector3(transform.position.x, transform.position.y, -10);
        Camera.main.transform.position = camera_position;
        UpdateSprite(move_vec);
    }

    void UpdateSprite(Vector2 move_vec){
    
        if(move_vec.x > 0){
            spriteRenderer.sprite = spriteList[characterIndex].right; 
            spriteRenderer.flipX = true;
        }

        if(move_vec.x < 0){
            spriteRenderer.flipX = false;
            spriteRenderer.sprite = spriteList[characterIndex].left; 
        }

        if(move_vec.y > 0){
            spriteRenderer.sprite = spriteList[characterIndex].back;  
        }

        if(move_vec.y < 0){
            spriteRenderer.sprite = spriteList[characterIndex].front;    
        }

    }

    void OnCollisionEnter2D(Collision2D collider) {
        var s = collider.gameObject.GetComponent<SceneObjectState>();
        var border = collider.gameObject.GetComponent<Border>();
        if (border){
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
        }
        else if (s) s.Destroy();
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        collider.gameObject.TryGetComponent<Item>(out possibleCollectItem);
    }

    private void OnTriggerExit2D(Collider2D collider) {
        if (possibleCollectItem != null && possibleCollectItem.gameObject == collider.gameObject) possibleCollectItem = null;
    }
}
