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
        Transform itemFound = null;
        Transform firstFreeSlot = null;
        IEnumerable<Transform> slots = fromIndex == 0 ? GetBagItemSlots() : GetChestSlots();
        foreach (Transform itemSlot in slots)
        {
            InventorySlot slot = itemSlot.GetComponent<InventorySlot>();
            if (slot.GetSlotItemName() == itemName)
            {
                itemFound = itemSlot;
                break;
            }
            if (slot.IsSlotEmpty() && firstFreeSlot == null)
            {
                firstFreeSlot = itemSlot;
            }
        }

        if (itemFound)
        {
            InventorySlot slot = itemFound.GetComponent<InventorySlot>();
            Item itemScript = slot.GetItem();
            if (itemScript is NonPermanentItem)
            {
                NonPermanentItem nonPermanentItem = (NonPermanentItem)itemScript;
                nonPermanentItem.IncreaseItemCount();
                slot.SetCount();
                return true;
            }
            else
            {
                return false;
            }
        }

        firstFreeSlot.GetComponent<InventorySlot>().SetSlot(itemFrom);
        return true;
    }


    public (Item, Item) AddItemInOtherLevel(string itemName, int slotIndex = 0, int fromIndex = 0)
    {
        if (!items.ContainsKey(itemName))
        {
            Debug.LogWarning($"Such an item ({itemName}) does not exist!");
            return (null, null);
        }

        var itemTo = items[itemName][slotIndex];
        Transform itemFound = null;
        Transform firstFreeSlot = null;
        IEnumerable<Transform> slots = fromIndex == 0 ? GetBagItemSlots() : GetChestSlots();
        foreach (Transform itemSlot in slots)
        {
            InventorySlot slot = itemSlot.GetComponent<InventorySlot>();
            if (slot.GetSlotItemName() == itemName)
            {
                itemFound = itemSlot;
                break;
            }
            if (slot.IsSlotEmpty() && firstFreeSlot == null)
            {
                firstFreeSlot = itemSlot;
            }
        }

        if (itemFound)
        {
            InventorySlot slot = itemFound.GetComponent<InventorySlot>();
            Item itemScript = slot.GetItem();

            // handle case if both items are non permanent
            if (itemScript is NonPermanentItem && itemTo is NonPermanentItem)
            {
                itemScript = HandleNonPermanentItem(itemScript, false, fromIndex);
                itemTo = HandleNonPermanentItem(itemTo, true, slotIndex);
                return (itemTo, itemScript);
            }
            else if (itemScript is NonPermanentItem)
            {
                itemScript = HandleNonPermanentItem(itemScript, false, fromIndex);
                itemTo = HandlePermanentItem(itemTo, slotIndex);
                return (itemTo, itemScript);
            }
            else if (itemTo is NonPermanentItem) {
                HandleNonPermanentItem(itemScript, true, fromIndex);
            } else 
            {
                // todo must be done right
                return (items["EmptyItem"][fromIndex], items["EmptyItem"][fromIndex]);
            }
        }

        firstFreeSlot.GetComponent<InventorySlot>().SetSlot(itemTo);
        return (itemTo, items["EmptyItem"][0]);
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

    public (Item, Item) SwapNonPermanentItems(Item itemScript, Item itemTo, InventorySlot slot, int fromIndex)
    {
        NonPermanentItem nonPermanentItem = (NonPermanentItem)itemScript;
        NonPermanentItem nonPermanentItemTo = (NonPermanentItem)itemTo;

        nonPermanentItem.DecreaseItemCount();
        if (nonPermanentItem.Count() < 1)
        {
            itemScript = items["EmptyItem"][fromIndex];
        }
        slot.SetCount();
        return (itemTo, itemScript);
    }



    public bool RemoveItem(string itemName, int slotIndex = 0)
    {
        if (!items.ContainsKey(itemName))
        {
            Debug.LogWarning($"Such an item ({itemName}) does not exist!");
            return false;
        }

        var item = items[itemName][slotIndex];
        Transform itemFound = null;

        IEnumerable<Transform> slots = slotIndex == 0 ? GetBagItemSlots() : GetChestSlots();
        foreach (Transform itemSlot in slots)
        {
            InventorySlot slot = itemSlot.GetComponent<InventorySlot>();
            if (slot.GetSlotItemName() == itemName)
            {
                itemFound = itemSlot;
                break;
            }
        }

        if (itemFound)
        {
            InventorySlot slot = itemFound.GetComponent<InventorySlot>();
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
                (itemTo, itemFrom) = AddItemInOtherLevel(firstItem.GetItemName(), 0, 1);

            }
            else if (firstItemSlot.IsBagSlot() && !secondItemSlot.IsBagSlot())
            {
                (itemTo, itemFrom) = AddItemInOtherLevel(firstItem.GetItemName(), 1, 0);
            } else {
                itemFrom = secondItem;
                itemTo = firstItem;
            }

            firstItemSlot.SetSlot(itemFrom);
            secondItemSlot.SetSlot(itemTo);
            uiManager.UpdateArmorStats();
            uiManager.OnAttemptSwapItems(selectedItemToMove, itemToMove);
            UnselectButtons();
        }
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
