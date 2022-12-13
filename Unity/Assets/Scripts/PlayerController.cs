using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] [SceneProperty] string testScene;
    
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

    private GameObject possibleCollectItem;

    void Start(){
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update(){
        MovePlayer();
        UseItem();
        InteractItem();
        CollectItem();
        Inventory();
        UpdatePlayerStatus();
    }

    void CollectItem() {
        Item collectItem = null;
        if (possibleCollectItem != null) possibleCollectItem.TryGetComponent<Item>(out collectItem);

        if (Input.GetButtonDown("CollectItem") && collectItem != null && collectItem.IsCollectible()) {
            // TODO add collision check and other collect features
            inventory.AddBagItem(possibleCollectItem.name);
            int sceneCount = SceneManager.sceneCount;
            for (int i = 0; i < sceneCount; i++)
            {
                if (SceneManager.GetSceneAt(i).path == testScene)
                    return;
            }
            possibleCollectItem.SetActive(false);
            possibleCollectItem = null;
        }
    }

    void UseItem() {
        if (Input.GetButtonDown("UseItem")) {
            inventory.UseEquippedItem(0);
        }
    }

    void InteractItem() {
        Item collectItem = null;
        if (possibleCollectItem != null) possibleCollectItem.TryGetComponent<Item>(out collectItem);

        if (Input.GetButtonDown("Interact") && collectItem != null && collectItem.IsInteractible()) {
            collectItem.UseItem();
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
