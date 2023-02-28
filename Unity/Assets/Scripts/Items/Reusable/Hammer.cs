using System;
using System.Collections;
using System.Collections.Generic;
using Items.Special;
using UnityEngine;

namespace Items.Reusable
{
    public class Hammer : PermanentItem
    {
        public const string HAMMER_COLLECTED = "hammerCollected";
        private float interactableArae = 1.2f;

        public override void UseItem()
        {
            Debug.Log("Trying to find nearby rocks...");
            Vector2 playerPosition = PlayerController.the().GetPosition();
            Collider2D[] colliders = Physics2D.OverlapCircleAll(playerPosition, interactableArae);
            
            foreach (Collider2D collider in colliders)
            {
                Rock rock = collider.GetComponent<Rock>();
                if (rock != null)
                {
                    Debug.Log("Found rocks nearby!");
                    //InventoryManager inventory = PlayerController.the().GetInventory();
                    PlayerController.the().GetComponent<Animator>().Play("attack_animation");
                    //if (!inventory.AddBagItem(rock)) return;
                    if (rock.gameObject.TryGetComponent<SceneObjectState>(out var os)) os.Destroy();
                    else Destroy(rock.gameObject);
                    break;
                }
            }
        }

        public override void OnCollect() {
            ProgressSystem.the().setProgress(HAMMER_COLLECTED);
        }

        public override string GetItemName() {
            return "Rock";
        }

        public override string GetItemDescription()
        {
            return "You might want to smash some rocks...";
        }
    }
}

