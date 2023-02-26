using System;
using System.Collections;
using System.Collections.Generic;
using Items.Special;
using UnityEngine;

namespace Items.Reusable
{
    public class Axe : PermanentItem
    {
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

                    Debug.Log("Found WOODLOGS!!!!!!!!!!! - Name: " + ((Item) woodLogs).name);
                    InventoryManager inventory = PlayerController.the().GetInventory();
                    if (!inventory.AddBagItem(woodLogs)) return;
                    if (woodLogs.gameObject.TryGetComponent<SceneObjectState>(out var os)) os.Destroy();
                    else Destroy(woodLogs.gameObject);
                    break;
                }
            }
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

