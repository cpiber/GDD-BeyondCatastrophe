using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryManager : GenericSingleton<InventoryManager>
{
    [SerializeField] SerializableDictionary<string, Item> items;
    [SerializeField] InventoryUIManager uiManager;
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
        if (uiManager.EquippedInventoryItems.childCount <= slotIndex) {
            Debug.LogWarning("Such an equiment slot does not exist!");
            return;
        }
        InventorySlot itemSlot = uiManager.EquippedInventoryItems.GetChild(slotIndex).GetComponent<InventorySlot>();
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

    public bool AddBagItem(Item collectItem) {
        string itemName = collectItem.name;
        if (!items.ContainsKey(itemName)) {
            Debug.LogWarning($"Such an item ({itemName}) does not exist!");
            return false;
        }

        var item = items[itemName];
        Debug.Assert(item.GetType() == collectItem.GetType(), "Excepted items to be of same type");
        Transform itemFound = null;
        Transform firstFreeSlot = null;
        foreach(Transform itemSlot in GetAllItemSlots()) {
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
            if (itemScript is NonPermanentItem nonPermanentItem) {
                nonPermanentItem.IncreaseItemCount(collectItem as NonPermanentItem);
                slot.SetCount();
                return true;
            } else {
                return false;
            }
        } 

        if (item is NonPermanentItem nonPermanentItem1) {
            nonPermanentItem1.SetUsages(collectItem as NonPermanentItem);
        }
        firstFreeSlot.GetComponent<InventorySlot>().SetSlot(item);
        return true;
    }

    public Item TakeBagItem(string itemName) {
        foreach(Transform itemSlot in GetAllItemSlots()) {
            InventorySlot slot = itemSlot.GetComponent<InventorySlot>();
            if (slot.GetSlotItemName() != itemName) continue;
            var item = slot.GetItem();
            if (item is NonPermanentItem perm) {
                Debug.Assert(perm.CanBeUsed());
                perm.DecreaseItemCount();
            }
            slot.SetCount();
            // if item has no use left => reset item slot to no item
            if (item is PermanentItem || !((NonPermanentItem)item).CanBeUsed()) {
                slot.ResetSlot();
            }
            return item;
        }
        return null;
    }

    public void SelectItemToMove(GameObject itemToMove) {
        if (!uiManager.IsUIOpen) {
            UnselectButtons();
            return;
        }

        if (selectedItemToMove == null) {
            // if first item is selected
            selectedItemToMove = itemToMove;
            uiManager.OnSelectSlotForMove(itemToMove);
        } else {
            // if second item is selected => swap
            InventorySlot firstItemSlot = selectedItemToMove.GetComponent<InventorySlot>();
            InventorySlot secondItemSlot = itemToMove.GetComponent<InventorySlot>();

            Item firstItem = firstItemSlot.GetItem();
            Item secondItem = secondItemSlot.GetItem();

            // check if item is swappable

            if (firstItemSlot.name.Contains("Armor") && !secondItem.IsArmor()) {
                UnselectButtons();
                uiManager.OnAttemptSwapItems(selectedItemToMove, itemToMove);
                return;
            } 
            if (secondItemSlot.name.Contains("Armor") && !firstItem.IsArmor()) {
                UnselectButtons();
                uiManager.OnAttemptSwapItems(selectedItemToMove, itemToMove);
                return;
            } 

            firstItemSlot.SetSlot(secondItem);
            secondItemSlot.SetSlot(firstItem);
            uiManager.UpdateArmorStats();
            uiManager.OnAttemptSwapItems(selectedItemToMove, itemToMove);
            UnselectButtons();
        }
    }

    public void UnselectButtons() {
        EventSystem.current.SetSelectedGameObject(null);
        selectedItemToMove = null;
    }

    // TODO This does a lot of calls to GetComponent<>, which is slow
    //      For future performance improvements, we might want to cache this
    private IEnumerable<Item> GetItemsFromSlot(Transform inv) {
        foreach (Transform child in inv) {
            var slot = child.gameObject.GetComponent<InventorySlot>();
            yield return slot.GetItem();
        }
    }

    public IEnumerable<Item> GetBagItems() {
        return GetItemsFromSlot(uiManager.BagInventoryItems);
    }
    public IEnumerable<Item> GetChestItems() {
        return GetItemsFromSlot(uiManager.ChestInventoryItems);
    }
    public IEnumerable<Item> GetArmorItems() {
        return GetItemsFromSlot(uiManager.ArmorInventoryItems);
    }
    public IEnumerable<Item> GetEquippedItems() {
        return GetItemsFromSlot(uiManager.EquippedInventoryItems);
    }

    public IEnumerable<Transform> GetAllItemSlots() {
        for (int i = 0; i < uiManager.BagInventoryItems.childCount; i++) yield return uiManager.BagInventoryItems.GetChild(i);
        for (int i = 0; i < uiManager.EquippedInventoryItems.childCount; i++) yield return uiManager.EquippedInventoryItems.GetChild(i);
        for (int i = 0; i < uiManager.ArmorInventoryItems.childCount; i++) yield return uiManager.ArmorInventoryItems.GetChild(i);
        // TODO: this allows picking up from anywhere...
        for (int i = 0; i < uiManager.ChestInventoryItems.childCount; i++) yield return uiManager.ChestInventoryItems.GetChild(i);
    }
}
