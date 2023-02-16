using System;
using UnityEngine;

public class MovementRenderController : MonoBehaviour
{
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
    public Animator animator;

    void Start(){
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void UpdateSprite(Vector2 move_vec){
        animator.SetFloat("walk_y", move_vec.y);
        animator.SetFloat("walk_x", move_vec.x);
        
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
}
