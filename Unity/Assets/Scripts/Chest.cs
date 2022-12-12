using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : PermanentItem
{
    [SerializeField] GameObject bagUI;
    [SerializeField] GameObject chestUI;
    [SerializeField] GameObject armorUI;
    [SerializeField] GameObject equippedUI;
    [SerializeField] List<Item> items;
    private bool isOpen = false;

    public void AddInventoryItem(Item item) {
        items.Add(item);
    }

    public void RemoveItem(Item item) {
        items.Remove(item);
    }

    public override void UseItem () {
        // open chest overlay (chest and inventory)
        if (isOpen) {
            Close();
        } else {
            Open();
        }
    }

    public void Open() {
        isOpen = true;
        bagUI.SetActive(true);
        chestUI.SetActive(true);
        armorUI.SetActive(true);
        equippedUI.SetActive(true);
    }

    public void Close() {
        isOpen = false;
        bagUI.SetActive(false);
        chestUI.SetActive(false);
        armorUI.SetActive(false);
        equippedUI.SetActive(false);
    }

    public bool IsOpen() {
        return isOpen;
    }

    public override string GetItemName() {
        return "Chest";
    }

    public override string GetItemDescription() {
        return "A classic chest which can hold items.";
    }
}
