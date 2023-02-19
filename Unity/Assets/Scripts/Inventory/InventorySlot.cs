using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] Item item;
    [SerializeField] Transform icon;
    [SerializeField] Item emptyItem;
    [SerializeField] GameObject usageLeft;
    [SerializeField] GameObject text;

    public void Start() {
        icon = transform.Find("ItemIcon");
    }

    public void ResetSlot() {
        SetSlot(emptyItem);
    }

    public string GetSlotItemName() {
        return item.GetItemName();
    }

    public string GetName() {
        return gameObject.name;
    }

    public void SetSlot(Item newItem) {
        item = newItem;

        Image iconImage = icon.gameObject.GetComponent<Image>();
        iconImage.sprite = item.GetSprite();
        icon.gameObject.SetActive(true);
        SetCount();
    }

    public void SetCount() {
        if (item is not NonPermanentItem) {
            usageLeft.SetActive(false); 
            return;
        }

        usageLeft.SetActive(true); 
        TextMeshProUGUI textElement = text.GetComponent<TextMeshProUGUI>();
        int amount = ((NonPermanentItem)item).Count();
        textElement.text = amount.ToString();
    }

    public GameObject GetContainer() {
        return usageLeft;
    }

    public GameObject GetTextElement() {
        return text;
    }

    public bool IsSlotEmpty() {
        return item.IsItemEmpty();
    }

    public Item GetItem() {
        return item;
    }

    public bool IsBagSlot() {
        return gameObject.name.Contains("Bag") || gameObject.name.Contains("Equipped") || gameObject.name.Contains("Armor") ? true : false;
    }

     public int IsBagSlotInt() {
        return gameObject.name.Contains("Bag") || gameObject.name.Contains("Equipped") || gameObject.name.Contains("Armor") ? 0 : 1;
    }

}
