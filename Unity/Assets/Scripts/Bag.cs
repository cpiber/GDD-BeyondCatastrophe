using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Bag : PermanentItem
{
    [SerializeField] GameObject bagInventoryItems;

    public override void UseItem () {
        // open chest overlay (chest and inventory)
        InventoryUIManager.the().ToggleBagUI();
    }

    public override string GetItemName() {
        return "Bag";
    }

    public override string GetItemDescription() {
        return "A classic bag which can hold items.";
    }

    public void AddBagItem(Item item) {
        Transform itemFound = null;
        Transform firstFreeSlot = null;
        string itemName = item.GetItemName();
        foreach(Transform itemSlot in bagInventoryItems.transform) {
            InventorySlot slot = itemSlot.GetComponent<InventorySlot>();
            if (slot.GetSlotItemName() == itemName) {
                itemFound = itemSlot;
                break;
            }
            if (slot.IsSlotEmpty() && firstFreeSlot == null) {
                firstFreeSlot = itemSlot;
            }
        }

        if (itemFound) {
            InventorySlot slot = itemFound.GetComponent<InventorySlot>();
            Item itemScript = slot.GetItem();
            System.Type test = itemScript.GetType();
            if (itemScript is NonPermanentItem) {
                NonPermanentItem nonPermanentItem = (NonPermanentItem)itemScript;
                nonPermanentItem.IncreaseItemCount();
                slot.SetCount();
            }
            return;
        } 

        firstFreeSlot.GetComponent<InventorySlot>().SetSlot(item);
    }
}
