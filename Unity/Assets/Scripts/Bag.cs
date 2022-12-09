using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bag : PermanentItem
{

    [SerializeField] GameObject bagInventoryItems;
    private bool isOpen = false;

    public override void UseItem () {
         if (isOpen) {
            Close();
        } else {
            Open();
        }
    }

    public override string GetItemName() {
        return "Bag";
    }

    public override string GetItemDescription() {
        return "A classic bag which can hold items.";
    }

    public bool IsOpen() {
        return isOpen;
    }

    public void Open() {
        isOpen = true;
        bagInventoryItems.SetActive(true);
    } 

    public void Close() {
        isOpen = false;
        bagInventoryItems.SetActive(false);
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
            
            Item itemScript = itemFound.GetComponent<InventorySlot>().GetItem();
            System.Type test = itemScript.GetType();
            if (itemScript is NonPermanentItem) {
                NonPermanentItem nonPermanentItem = (NonPermanentItem)itemScript;
                nonPermanentItem.IncreaseItemCount();
            }
            return;
        } 

        firstFreeSlot.GetComponent<InventorySlot>().SetSlot(item);
    }
}
