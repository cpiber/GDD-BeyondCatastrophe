using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] SerializableDictionary<string, Item> items;
    [SerializeField] GameObject bagInventoryItems;
    [SerializeField] GameObject chestInventoryItems;
    [SerializeField] Chest chest;
    [SerializeField] Bag bag;
    private GameObject selectedItemToMove = null;


    void Start() {
        GameObject itemsObjects = GameObject.Find("Items");
        items = new SerializableDictionary<string, Item>();
        foreach (Transform item in itemsObjects.transform) {
            items[item.gameObject.name] = item.gameObject.GetComponent<Item>();
        }
    }

    public void UseBag() {
        if (!chest.IsOpen()) {
            bag.UseItem();
        }
        selectedItemToMove = null;
    }

    public void UseChest() {
        if (!bag.IsOpen()) {
            chest.UseItem();
        }
        selectedItemToMove = null;
    }

    public void UseItem(string itemName) {
        Item itemToUse = items[itemName];
        itemToUse.UseItem();
    }  

    public void AddBagItem(string itemName) {
       bag.AddBagItem(items[itemName]);
    }

    public void SelectItemToMove(GameObject itemToMove) {
        Debug.Log("Selected item to move");
        if (selectedItemToMove == null) {
            // if first item is selected
            selectedItemToMove = itemToMove;
        } else {
            // if second item is selected => swap
            InventorySlot firstItemSlot = selectedItemToMove.GetComponent<InventorySlot>();
            InventorySlot secondItemSlot = itemToMove.GetComponent<InventorySlot>();

            Item firstItem = firstItemSlot.GetItem();
            Item secondItem = secondItemSlot.GetItem();

            firstItemSlot.SetSlot(secondItem);
            secondItemSlot.SetSlot(firstItem);

            selectedItemToMove = null;
        }
    }
}
