using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] int health = 100;
    [SerializeField] int tiredness = 0;
    [SerializeField] int hunger = 0;
    [SerializeField] int thirst = 0;

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

    private bool isItemButtonPressed = false;
    private bool isItemInteractPressed = false;
    private bool isItemCollectPressed = false;
    private bool isItemOpenMenuPressed = false;

    private GameObject possibleCollectItem;

    void Start(){
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update(){
        MovePlayer();
        UseItem();
        Inventory();
        CollectItem();
        UpdatePlayerStatus();
    }

    void CollectItem() {
        if (Input.GetAxis("CollectItem") > 0 && possibleCollectItem != null) {
            // TODO add collision check and other collect features
            isItemCollectPressed = true;
            inventory.AddBagItem(possibleCollectItem.name);
            possibleCollectItem.SetActive(false);
            possibleCollectItem = null;
        } else {
            isItemCollectPressed = false;
        }
    }

    void UseItem() {
        if (Input.GetAxis("UseItem") > 0){
            if (!isItemButtonPressed) {
                isItemButtonPressed = true;
                inventory.UseItem("Apple");
            }
        } else {
            isItemButtonPressed = false;
        }
    }

    void Inventory() {
        if (Input.GetAxis("Interact") > 0){
            if (!isItemInteractPressed) {
                isItemInteractPressed = true;
                inventory.UseItem("Bag");
            }
        } else {
            isItemInteractPressed = false;
        }

        if (Input.GetAxis("OpenMenu") > 0) {
            if (!isItemOpenMenuPressed) {
                if (inventory.IsOpen()) {
                    isItemOpenMenuPressed = true;
                    inventory.Close();
                } else {
                    isItemOpenMenuPressed = true;
                    inventory.Open();
                }
            }
        } else {
            isItemOpenMenuPressed = false;
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
        possibleCollectItem = collider.gameObject;
    }

    private void OnTriggerExit2D(Collider2D collider) {
        possibleCollectItem = null;
    }

    void UpdatePlayerStatus(){
        if(health <= 0){
            Destroy(gameObject);
            // TODO: load menue/end-screen
        }

        if(tiredness > 100){
            // TODO: do something
        }


        if(hunger > 100){
            // TODO: do something
        }


        if(thirst > 100){
            // TODO: do something
        }
    }

}
