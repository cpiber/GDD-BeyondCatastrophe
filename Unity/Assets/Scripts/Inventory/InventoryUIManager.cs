using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryUIManager : GenericSingleton<InventoryUIManager>
{
    public const int MAX_EQUIPPED_ITEMS = 3;
    public const string KEYBOARD_SCHEME = "Keyboard&Mouse";

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
    [SerializeField] PlayerInput playerInput;
    Transform equippedActualItems => equippedUI.transform.GetChild(1);

    [Header("Equipped slots")]
    [SerializeField] Color equippedSelectedCol = new Color32(0, 0, 0, 123);
    [SerializeField] Color equippedUnselectedCol = new Color32(255, 255, 255, 123);
    [SerializeField] Color slotSelectedForMoveCol = new Color32(100, 0, 0, 123);
    private int useEquippedItemIndex = 0;
    public int UseEquippedItemIndex => useEquippedItemIndex;
    
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
    public bool IsUIOpen => openUI != UI.Closed;
    public bool ShouldInhibitMovement => IsUIOpen && playerInput.currentControlScheme != KEYBOARD_SCHEME;

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
        if (dialogueSystem.IsOpen) dialogueSystem.CancelDialogue();
        // Allow to open bag regardless of whether bag is open
        if (openUI == UI.Chest) CloseAllUI();
        else {
            HideHotbar();
            bagUI.SetActive(true);
            chestUI.SetActive(true);
            armorUI.SetActive(true);
            equippedUI.SetActive(true);
            hud.SetActive(false);
            StartCoroutine(SelectSelectedIfNecessary());
            openUI = UI.Chest;
        }
    }
    
    [ContextMenu("Toggle Bag")]
    public void ToggleBagUI() {
        if (dialogueSystem.IsOpen) dialogueSystem.CancelDialogue();
        // Bag toggles Chest as well
        if (openUI != UI.Closed) CloseAllUI();
        else {
            HideHotbar();
            bagUI.SetActive(true);
            armorUI.SetActive(true);
            equippedUI.SetActive(true);
            hud.SetActive(false);
            StartCoroutine(SelectSelectedIfNecessary());
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

    public void OnAttemptSwapItems(GameObject source, GameObject destination) {
        source.GetComponent<Image>().color = equippedUnselectedCol;
        StartCoroutine(SelectSelectedIfNecessary(destination));
    }

    public void OnSelectSlotForMove(GameObject slot) {
        slot.GetComponent<Image>().color = slotSelectedForMoveCol;
    }

    public void SetSelectedSlot(int index) {
        Debug.Assert(0 <= index && index < 3);
        if (IsUIOpen) return;
        useEquippedItemIndex = index;

        for (int i = 0; i < MAX_EQUIPPED_ITEMS; i++) {
            InventorySlot itemSlot = GetInventorySlot(i);
            itemSlot.GetComponent<Image>().color = i == index ? equippedSelectedCol : equippedUnselectedCol;
        }
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

        for (int i = 0; i < MAX_EQUIPPED_ITEMS; i++) {
            InventorySlot itemSlot = GetInventorySlot(i);
            itemSlot.GetComponent<Image>().color = i == useEquippedItemIndex ? equippedSelectedCol : equippedUnselectedCol;
        }
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

        for (int i = 0; i < MAX_EQUIPPED_ITEMS; i++) {
            InventorySlot itemSlot = GetInventorySlot(i);
            itemSlot.GetComponent<Image>().color = equippedUnselectedCol;
        }
    }

    private IEnumerator SelectSelectedIfNecessary(GameObject obj = null) {
        if (playerInput.currentControlScheme == KEYBOARD_SCHEME) yield break;
        if (obj == null) obj = BagInventoryItems.GetChild(0).gameObject;
        yield return null;
        EventSystem.current.SetSelectedGameObject(obj);
    }

    private IEnumerator Relayout() {
        // Apparently the InventoryUIGrid from the equippedUI interferes here? Probably still has references somewhere
        yield return null;
        LayoutRebuilder.ForceRebuildLayoutImmediate(hotbar.GetComponent<RectTransform>());
    }
    
    private InventorySlot GetInventorySlot(int slotIndex) {
        return EquippedInventoryItems.GetChild(slotIndex).GetComponent<InventorySlot>();
    }
}
