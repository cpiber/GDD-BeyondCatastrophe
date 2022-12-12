using UnityEngine;

public class InventoryUIManager : MonoBehaviour
{
    [SerializeField] GameObject bagUI;
    [SerializeField] GameObject chestUI;
    [SerializeField] GameObject armorUI;
    [SerializeField] GameObject equippedUI;
    [SerializeField] GameObject itemDescUI;
    [SerializeReference] static InventoryUIManager instance = null;

    public static InventoryUIManager the() {
        if (instance == null) instance = FindObjectOfType<InventoryUIManager>();
        return instance;
    }
    
    enum UI {
        Closed,
        Bag,
        Chest,
    }
    private UI openUI;
    
    public void ToggleChestUI() {
        // Allow to open bag regardless of whether bag is open
        if (openUI == UI.Chest) CloseAllUI();
        else {
            bagUI.SetActive(true);
            chestUI.SetActive(true);
            armorUI.SetActive(true);
            equippedUI.SetActive(true);
            openUI = UI.Chest;
        }
    }
    
    public void ToggleBagUI() {
        // Bag toggles Chest as well
        if (openUI != UI.Closed) CloseAllUI();
        else {
            bagUI.SetActive(true);
            armorUI.SetActive(true);
            equippedUI.SetActive(true);
            openUI = UI.Bag;
        }
    }

    public void CloseAllUI() {
        bagUI.SetActive(false);
        chestUI.SetActive(false);
        armorUI.SetActive(false);
        equippedUI.SetActive(false);
        openUI = UI.Closed;
    }
}
