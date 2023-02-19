using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryManager : GenericSingleton<InventoryManager>
{
    [SerializeField] SerializableDictionary<string, List<Item>> items;


    [SerializeField] InventoryUIManager uiManager;
    private GameObject selectedItemToMove = null;

    void Start()
    {
        GameObject itemsObjects = GameObject.Find("Items");
        items = new SerializableDictionary<string, List<Item>>();

        foreach (Transform item in itemsObjects.transform)
        {
            List<Item> chestAndBagItemList = new List<Item>();
            chestAndBagItemList.Add(item.gameObject.GetComponent<Item>());
            items[item.gameObject.name] = chestAndBagItemList;
        }

        itemsObjects = GameObject.Find("ChestItems");

        foreach (Transform item in itemsObjects.transform)
        {
            List<Item> test = items[item.gameObject.name];
            List<Item> test2 = items[item.gameObject.name];

            items[item.gameObject.name].Add(item.gameObject.GetComponent<Item>());
        }
    }

    // list is 0 indexed (three weapons => 0,1,2 slot)
    public void UseEquippedItem(int slotIndex)
    {
        if (uiManager.EquippedInventoryItems.childCount <= slotIndex)
        {
            Debug.LogWarning("Such an equiment slot does not exist!");
            return;
        }
        InventorySlot itemSlot = uiManager.EquippedInventoryItems.GetChild(slotIndex).GetComponent<InventorySlot>();
        Item itemToUse = itemSlot.GetItem();
        itemToUse.UseItem();
        itemSlot.SetCount();

        // if item has no use left => reset item slot to no item
        if (!itemToUse.IsReusable())
        {
            NonPermanentItem item = (NonPermanentItem)itemToUse;
            // if item is out of usage => remove item from inventory
            if (!item.CanBeUsed())
            {
                itemSlot.ResetSlot();
            }
        }
    }

    public void UseBag()
    {
        InventoryUIManager.the().ToggleBagUI();
        UnselectButtons();
    }

    public void UseChest()
    {
        InventoryUIManager.the().ToggleChestUI();
        UnselectButtons();
    }

    public void UseItem(string itemName)
    {
        if (!items.ContainsKey(itemName))
        {
            Debug.LogWarning($"Such an item ({itemName}) does not exist!");
            return;
        }
        // zero index for bag items
        Item itemToUse = items[itemName][0];
        itemToUse.UseItem();
    }

    public bool AddItem(string itemName, int slotIndex = 0, int fromIndex = 0)
    {
        if (!items.ContainsKey(itemName))
        {
            Debug.LogWarning($"Such an item ({itemName}) does not exist!");
            return false;
        }

        var itemFrom = items[itemName][slotIndex];
        InventorySlot slot = GetSlot(itemFrom.GetItemName(), fromIndex, true);
        
        if (slot)
        {
            Item itemScript = slot.GetItem();
            if (itemScript is NonPermanentItem)
            {
                NonPermanentItem nonPermanentItem = (NonPermanentItem)itemScript;
                nonPermanentItem.IncreaseItemCount();
                slot.SetCount();
                return true;
            }
        }

        slot.GetComponent<InventorySlot>().SetSlot(itemFrom);
        return true;
    }


    public void AddItemInOtherLevel(InventorySlot slotFrom, InventorySlot slotTo, string itemName, int slotIndex = 0, int fromIndex = 0)
    {
        if (!items.ContainsKey(itemName))
        {
            Debug.LogWarning($"Such an item ({itemName}) does not exist!");
            return;
        }

        Item firstItem = slotFrom.GetItem();
        Item secondItem = slotTo.GetItem();

        // pure swap of items
        if (firstItem.GetItemName() != secondItem.GetItemName() && !secondItem.IsItemEmpty()) {
            slotFrom.SetSlot(secondItem);
            slotTo.SetSlot(firstItem);
            return;
        }

        var itemTo = items[itemName][slotIndex];

        InventorySlot slot = GetSlot(itemTo.GetItemName(), slotIndex, true);

        if (slot)
        {
            Item itemFrom = slotFrom.GetItem();

            // handle case if both items are non permanent
            if (itemFrom is NonPermanentItem && itemTo is NonPermanentItem)
            {
                itemFrom = HandleNonPermanentItem(itemFrom, false, fromIndex);
                itemTo = HandleNonPermanentItem(itemTo, true, slotIndex);
                slotFrom.SetSlot(itemFrom);
                slot.SetSlot(itemTo);
                return;
            }
            else if (itemFrom is NonPermanentItem)
            {
                itemFrom = HandleNonPermanentItem(itemFrom, false, fromIndex);
                itemTo = HandlePermanentItem(itemTo, slotIndex);
                slotFrom.SetSlot(itemFrom);
                slot.SetSlot(itemTo);
                return;
            }
            else if (itemTo is NonPermanentItem) {
                itemFrom = HandlePermanentItem(itemFrom, slotIndex);
                itemTo = HandleNonPermanentItem(itemTo, true, fromIndex);

                slotFrom.SetSlot(itemFrom);
                slot.SetSlot(itemTo);
            }
        }
        
        slotFrom.SetSlot(items["EmptyItem"][fromIndex]);
        slotTo.SetSlot(itemTo);
        return;
    }

    public Item HandlePermanentItem(Item item, int index)
    {
        return items["EmptyItem"][index];
    }

    public Item HandleNonPermanentItem(Item item, bool increase, int index)
    {
        NonPermanentItem nonPermanentItem = (NonPermanentItem)item;
        if (increase)
        {
            nonPermanentItem.IncreaseItemCount();
        }
        else
        {
            nonPermanentItem.DecreaseItemCount();
            if (nonPermanentItem.Count() < 1)
            {
                return items["EmptyItem"][index];
            }
        }
        return item;
    }

    public bool RemoveItem(string itemName, int slotIndex = 0)
    {
        if (!items.ContainsKey(itemName))
        {
            Debug.LogWarning($"Such an item ({itemName}) does not exist!");
            return false;
        }

        var item = items[itemName][slotIndex];
        InventorySlot slot = GetSlot(item.GetItemName(), slotIndex, false);

        if (slot)
        {
            Item itemScript = slot.GetItem();
            if (itemScript is NonPermanentItem)
            {
                NonPermanentItem nonPermanentItem = (NonPermanentItem)itemScript;
                nonPermanentItem.ResetItemCount();
                slot.SetCount();
                return true;
            }
            else
            {
                slot.SetSlot(items["EmptyItem"][slotIndex]);
            }
        }

        return true;
    }


    public void SelectItemToMove(GameObject itemToMove)
    {
        if (!uiManager.IsUIOpen)
        {
            UnselectButtons();
            return;
        }

        if (selectedItemToMove == null)
        {
            // if first item is selected
            selectedItemToMove = itemToMove;
            uiManager.OnSelectSlotForMove(itemToMove);
        }
        else
        {
            // if second item is selected => swap
            InventorySlot firstItemSlot = selectedItemToMove.GetComponent<InventorySlot>();
            InventorySlot secondItemSlot = itemToMove.GetComponent<InventorySlot>();

            Item firstItem = firstItemSlot.GetItem();
            Item secondItem = secondItemSlot.GetItem();

            // check if item is swappable

            if (firstItemSlot.name.Contains("Armor") && !secondItem.IsArmor())
            {
                UnselectButtons();
                uiManager.OnAttemptSwapItems(selectedItemToMove, itemToMove);
                return;
            }
            if (secondItemSlot.name.Contains("Armor") && !firstItem.IsArmor())
            {
                UnselectButtons();
                uiManager.OnAttemptSwapItems(selectedItemToMove, itemToMove);
                return;
            }

            // get name so we can check if it is a bag item or a chest item
            string firstSlotName = firstItemSlot.gameObject.name;
            string secondSlotName = secondItemSlot.gameObject.name;

            Item itemTo = items["EmptyItem"][0];
            Item itemFrom = items["EmptyItem"][0];


            // swap from bag to chest or from chest to bag
            if ((!firstItemSlot.IsBagSlot() && secondItemSlot.IsBagSlot())
                )
            {
                AddItemInOtherLevel(firstItemSlot, secondItemSlot, firstItem.GetItemName(), 0, 1);
            }
            else if (firstItemSlot.IsBagSlot() && !secondItemSlot.IsBagSlot())
            {
                AddItemInOtherLevel(firstItemSlot, secondItemSlot, firstItem.GetItemName(), 1, 0);
            } else {
                firstItemSlot.SetSlot(secondItem);
                secondItemSlot.SetSlot(firstItem);
            }

            uiManager.UpdateArmorStats();
            uiManager.OnAttemptSwapItems(selectedItemToMove, itemToMove);
            UnselectButtons();
        }
    }

    public InventorySlot GetSlot(string name, int isBagSlot, bool returnFreeSlotIfNotFound) {

        // if slot to does not hold right item => return right slot

        IEnumerable<Transform> slots = isBagSlot == 0 ? GetBagItemSlots() : GetChestSlots();
        InventorySlot freeSlot = null;
        foreach (Transform itemSlot in slots) {
            InventorySlot slot = itemSlot.GetComponent<InventorySlot>();
            if (slot.GetSlotItemName() == name)
            {
                return slot;
            }

            if (freeSlot == null && slot.IsSlotEmpty())
            {
                freeSlot = slot;
            }
        }

        return returnFreeSlotIfNotFound ? freeSlot : null;
    }

    public void UnselectButtons()
    {
        EventSystem.current.SetSelectedGameObject(null);
        selectedItemToMove = null;
    }

    // TODO This does a lot of calls to GetComponent<>, which is slow
    //      For future performance improvements, we might want to cache this
    private IEnumerable<Item> GetItemsFromSlot(Transform inv)
    {
        foreach (Transform child in inv)
        {
            var slot = child.gameObject.GetComponent<InventorySlot>();
            yield return slot.GetItem();
        }
    }

    public IEnumerable<Item> GetBagItems()
    {
        return GetItemsFromSlot(uiManager.BagInventoryItems);
    }
    public IEnumerable<Item> GetChestItems()
    {
        return GetItemsFromSlot(uiManager.ChestInventoryItems);
    }
    public IEnumerable<Item> GetArmorItems()
    {
        return GetItemsFromSlot(uiManager.ArmorInventoryItems);
    }
    public IEnumerable<Item> GetEquippedItems()
    {
        return GetItemsFromSlot(uiManager.EquippedInventoryItems);
    }

    public IEnumerable<Transform> GetBagItemSlots()
    {
        for (int i = 0; i < uiManager.BagInventoryItems.childCount; i++) yield return uiManager.BagInventoryItems.GetChild(i);
        for (int i = 0; i < uiManager.EquippedInventoryItems.childCount; i++) yield return uiManager.EquippedInventoryItems.GetChild(i);
        for (int i = 0; i < uiManager.ArmorInventoryItems.childCount; i++) yield return uiManager.ArmorInventoryItems.GetChild(i);
    }

    public IEnumerable<Transform> GetChestSlots()
    {
        for (int i = 0; i < uiManager.ChestInventoryItems.childCount; i++) yield return uiManager.ChestInventoryItems.GetChild(i);
    }
}
