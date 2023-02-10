using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : GenericSingleton<InventoryUIManager>
{
    [SerializeField] GameObject bagUI;
    [SerializeField] GameObject chestUI;
    [SerializeField] GameObject armorUI;
    [SerializeField] GameObject equippedUI;
    [SerializeField] GameObject itemDescUI;
    [SerializeField] GameObject hotbarEquippedUI;
    [SerializeField] TMP_Text hotbarArmorUI;
    [SerializeField] GameObject hud;
    [SerializeField] GameObject hotbar;
    [SerializeField] DialogueSystem dialogueSystem;
    Transform equippedActualItems => equippedUI.transform.GetChild(1);
    
    public Transform BagInventoryItems => bagUI.transform.GetChild(1);
    public Transform ChestInventoryItems => chestUI.transform.GetChild(1);
    public Transform ArmorInventoryItems => armorUI.transform.GetChild(1);
    public Transform EquippedInventoryItems => hotbar.activeSelf ? hotbarEquippedUI.transform : equippedActualItems;
    
    enum UI {
        Closed,
        Bag,
        Chest,
    }
    private UI openUI;

    void Start() {
        // Dummy items for UI
        for (int i = 0; i < hotbarEquippedUI.transform.childCount; i++) {
            Destroy(hotbarEquippedUI.transform.GetChild(i).gameObject);
        }
        ShowHotbar(true);
        UpdateArmorStats();
    }
    
    [ContextMenu("Toggle Chest")]
    public void ToggleChestUI() {
        if (dialogueSystem.IsOpen) return;
        // Allow to open bag regardless of whether bag is open
        if (openUI == UI.Chest) CloseAllUI();
        else {
            HideHotbar();
            bagUI.SetActive(true);
            chestUI.SetActive(true);
            armorUI.SetActive(true);
            equippedUI.SetActive(true);
            hud.SetActive(false);
            openUI = UI.Chest;
        }
    }
    
    [ContextMenu("Toggle Bag")]
    public void ToggleBagUI() {
        if (dialogueSystem.IsOpen) return;
        // Bag toggles Chest as well
        if (openUI != UI.Closed) CloseAllUI();
        else {
            HideHotbar();
            bagUI.SetActive(true);
            armorUI.SetActive(true);
            equippedUI.SetActive(true);
            hud.SetActive(false);
            openUI = UI.Bag;
        }
    }

    [ContextMenu("Close All UI")]
    public void CloseAllUI() {
        bagUI.SetActive(false);
        chestUI.SetActive(false);
        armorUI.SetActive(false);
        equippedUI.SetActive(false);
        hud.SetActive(true);
        ShowHotbar();
        openUI = UI.Closed;
    }

    [ContextMenu("Update Armor Stats")]
    public void UpdateArmorStats() {
        var buff = StatusSystem.the().TemperatureBuffs;
        hotbarArmorUI.text = (buff >= 0 ? "+" : "") + buff.ToString("N0");
    }

    [ContextMenu("Show Hotbar")]
    private void ShowHotbar(bool force = false) {
        // Don't mess anything up!
        if (hotbar.activeSelf && !force) return;
        Debug.Assert(equippedActualItems.childCount > 0);
        Debug.Assert(force || hotbarEquippedUI.transform.childCount == 0);
        while (equippedActualItems.childCount > 0) {
            equippedActualItems.GetChild(0).SetParent(hotbarEquippedUI.transform, false);
        }
        hotbar.SetActive(true);
        StartCoroutine(Relayout());
    }

    [ContextMenu("Hide Hotbar")]
    private void HideHotbar() {
        // Don't mess anything up!
        if (!hotbar.activeSelf) return;
        Debug.Assert(equippedActualItems.childCount == 0);
        Debug.Assert(hotbarEquippedUI.transform.childCount > 0);
        hotbar.SetActive(false);
        while (hotbarEquippedUI.transform.childCount > 0) {
            hotbarEquippedUI.transform.GetChild(0).SetParent(equippedActualItems, false);
        }
        equippedActualItems.GetComponent<InventoryUIGrid>().SetLayoutVertical();
    }

    private IEnumerator Relayout() {
        // Apparently the InventoryUIGrid from the equippedUI interferes here? Probably still has references somewhere
        yield return null;
        LayoutRebuilder.ForceRebuildLayoutImmediate(hotbar.GetComponent<RectTransform>());
    }
}
