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

    public void UseItem(string itemName) {
        Item itemToUse = items[itemName];
        itemToUse.UseItem();
    }  
}
