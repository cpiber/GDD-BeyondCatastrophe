using System;
using System.Collections;
using System.Collections.Generic;
using Items.Special;
using UnityEngine;

namespace Items.Reusable
{
    public class Axe : PermanentItem
    {
        public const string AXE_COLLECTED = "axeCollected";
        private float interactableArae = 1.2f;

        public override void UseItem()
        {
            Debug.Log("Trying to find nearby logs...");
            Vector2 playerPosition = PlayerController.the().GetPosition();
            Collider2D[] colliders = Physics2D.OverlapCircleAll(playerPosition, interactableArae);
            
            foreach (Collider2D collider in colliders)
            {
                WoodLogs woodLogs = collider.GetComponent<WoodLogs>();
                if (woodLogs != null)
                {
                    Debug.Log("Found Woodlogs nearby!");
                    InventoryManager inventory = PlayerController.the().GetInventory();
                    PlayerController.the().GetComponent<Animator>().Play("attack_animation");
                    if (!inventory.AddBagItem(woodLogs)) return;
                    if (woodLogs.gameObject.TryGetComponent<SceneObjectState>(out var os)) os.Destroy();
                    else Destroy(woodLogs.gameObject);
                    break;
                }
            }
        }

        public override void OnCollect() {
            ProgressSystem.the().setProgress(AXE_COLLECTED);
        }

        public override string GetItemName() {
            return "Axe";
        }

        public override string GetItemDescription()
        {
            return "The axe can help you chop down small trees and split up logs. Somehow Pitbull" +
                   " and Kesha come to mind...";
                   //Remember to yell, 'Timber'! - Otherwise, Pitbull and Ke$ha will be mad...";
        }
    }
}

