using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryManager : GenericSingleton<InventoryManager>
{
    [SerializeField] SerializableDictionary<string, Item> items;
    [SerializeField] GameObject bagInventoryItems;
    [SerializeField] GameObject chestInventoryItems;
    [SerializeField] Bag bag;
    [SerializeField] InventorySlot[] equippedItems;
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
        if (equippedItems.Length <= slotIndex) {
            Debug.LogWarning("Such an equiment slot does not exist!");
            return;
        }
        InventorySlot itemSlot = equippedItems[slotIndex];
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
}
