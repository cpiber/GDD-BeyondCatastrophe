using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] Item item;
    [SerializeField] Transform icon;

    public void Start() {
        icon = transform.Find("ItemIcon");
    }

    public void ResetSlot() {
        item = null;

    }

    public string GetSlotItemName() {
        return item.GetItemName();
    }

    public void SetSlot(Item newItem) {
        item = newItem;

        Image iconImage = icon.gameObject.GetComponent<Image>();
        iconImage.sprite = item.GetComponent<Item>().GetSprite();
        icon.gameObject.SetActive(true);
    }

    public bool IsSlotEmpty() {
        return item.IsItemEmpty();
    }

    public Item GetItem() {
        return item;
    }

}
