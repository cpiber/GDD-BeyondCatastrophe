using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] Item item;

    public void ResetSlot() {
        item = null;
    }

    public string GetSlotItemName() {
        return item.GetItemName();
    }

    public void SetSlot(Item newItem) {
        item = newItem;
    }

    public bool IsSlotEmpty() {
        return item.IsItemEmpty();
    }
}
