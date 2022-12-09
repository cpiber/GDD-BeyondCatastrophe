using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    void Start() {
        GameObject itemsObjects = GameObject.Find("Items");
        items = new SerializableDictionary<string, Item>();
        foreach (Transform item in itemsObjects.transform) {
            items[item.gameObject.name] = item.gameObject.GetComponent<Item>();
        }
    }

    [SerializeField] SerializableDictionary<string, Item> items;
    [SerializeField] GameObject bagInventoryItems;
    [SerializeField] GameObject chestInventoryItems;
    private GameObject selectedItemToMove = null;

    private bool isOpen = false;

    public void Open() {
        bagInventoryItems.SetActive(true);
        isOpen = true;
    }

    public void Close() {
        bagInventoryItems.SetActive(false);
        isOpen = false;
    }

    public bool IsOpen() {
        return isOpen;
    }

    public void UseItem(string itemName) {
        Item itemToUse = items[itemName];
        itemToUse.UseItem();
    }  

    public void AddBagItem(string itemName) {
        Transform itemFound = null;
        Transform firstFreeSlot = null;
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

        // set free item slot to apple
        firstFreeSlot.GetComponent<InventorySlot>().SetSlot(items[itemName]);
        return;
        // if item is first time in inventory
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
