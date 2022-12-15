using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : PermanentItem
{
    [SerializeField] List<Item> items;

    public void AddInventoryItem(Item item) {
        items.Add(item);
    }

    public void RemoveItem(Item item) {
        items.Remove(item);
    }

    public override void UseItem () {
        // open chest overlay (chest and inventory)
        InventoryManager.the().UseChest();
    }

    public override string GetItemName() {
        return "Chest";
    }

    public override string GetItemDescription() {
        return "A classic chest which can hold items.";
    }

    public override bool IsInteractible() {
        return true;
    }

    public override bool IsCollectible() {
        return false;
    }
}
