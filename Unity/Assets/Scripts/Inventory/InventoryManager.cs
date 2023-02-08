using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryManager : GenericSingleton<InventoryManager>
{
    [SerializeField] SerializableDictionary<string, Item> items;
    [SerializeField] GameObject bagInventoryItems;
    [SerializeField] GameObject chestInventoryItems;
    [SerializeField] GameObject armorInventoryItems;
    [SerializeField] GameObject equippedInventoryItems;
    [SerializeField] Bag bag;
    private GameObject selectedItemToMove = null;

    void Start() {
        GameObject itemsObjects = GameObject.Find("Items");
        items = new SerializableDictionary<string, Item>();
        foreach (Transform item in itemsObjects.transform) {
            items[item.gameObject.name] = item.gameObject.GetComponent<Item>();
        }
    }

    // list is 0 indexed (three weapons => 0,1,2 slot)
    public void UseEquippedItem(int slotIndex) {
        if (equippedInventoryItems.transform.childCount <= slotIndex) {
            Debug.LogWarning("Such an equiment slot does not exist!");
            return;
        }
        InventorySlot itemSlot = equippedInventoryItems.transform.GetChild(slotIndex).GetComponent<InventorySlot>();
        Item itemToUse = itemSlot.GetItem();
        itemToUse.UseItem();
        itemSlot.SetCount();

        // if item has no use left => reset item slot to no item
        if (!itemToUse.IsReusable()) {
            NonPermanentItem item = (NonPermanentItem)itemToUse;
            // if item is out of usage => remove item from inventory
            if (!item.CanBeUsed()) {
                itemSlot.ResetSlot();
            }
        } 
    }   

    public void UseBag() {
        InventoryUIManager.the().ToggleBagUI();
        UnselectButtons();
    }

    public void UseChest() {
        InventoryUIManager.the().ToggleChestUI();
        UnselectButtons();
    }

    public void UseItem(string itemName) {
        if (!items.ContainsKey(itemName)) {
            Debug.LogWarning($"Such an item ({itemName}) does not exist!");
            return;
        }
        Item itemToUse = items[itemName];
        itemToUse.UseItem();
    }  

    public void AddBagItem(string itemName) {
        if (!items.ContainsKey(itemName)) {
            Debug.LogWarning($"Such an item ({itemName}) does not exist!");
            return;
        }
        bag.AddBagItem(items[itemName]);
    }

    public void SelectItemToMove(GameObject itemToMove) {
        if (selectedItemToMove == null) {
            // if first item is selected
            selectedItemToMove = itemToMove;
        } else {
            // if second item is selected => swap
            InventorySlot firstItemSlot = selectedItemToMove.GetComponent<InventorySlot>();
            InventorySlot secondItemSlot = itemToMove.GetComponent<InventorySlot>();

            Item firstItem = firstItemSlot.GetItem();
            Item secondItem = secondItemSlot.GetItem();

            // check if item is swappable

            if (firstItemSlot.name.Contains("Armor") && !secondItem.IsArmor()) {
                UnselectButtons();
                return;
            } 
            if (secondItemSlot.name.Contains("Armor") && !firstItem.IsArmor()) {
                UnselectButtons();
                return;
            } 

            firstItemSlot.SetSlot(secondItem);
            secondItemSlot.SetSlot(firstItem);
            UnselectButtons();
        }
    }

    public void UnselectButtons() {
        EventSystem.current.SetSelectedGameObject(null);
        selectedItemToMove = null;
    }

    // TODO This does a lot of calls to GetComponent<>, which is slow
    //      For future performance improvements, we might want to cache this
    private IEnumerable<Item> GetItemsFromSlot(GameObject inv) {
        foreach (Transform child in armorInventoryItems.transform) {
            var slot = child.gameObject.GetComponent<InventorySlot>();
            yield return slot.GetItem();
        }
    }

    public IEnumerable<Item> GetBagItems() {
        return GetItemsFromSlot(bagInventoryItems);
    }
    public IEnumerable<Item> GetChestItems() {
        return GetItemsFromSlot(chestInventoryItems);
    }
    public IEnumerable<Item> GetArmorItems() {
        return GetItemsFromSlot(armorInventoryItems);
    }
    public IEnumerable<Item> GetEquippedItems() {
        return GetItemsFromSlot(equippedInventoryItems);
    }

    public InventorySlot GetInventorySlot(int slotIndex) {
        return equippedInventoryItems.transform.GetChild(slotIndex).GetComponent<InventorySlot>();
    }
}
