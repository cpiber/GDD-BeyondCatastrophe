using UnityEngine;

public class InventoryUIManager : GenericSingleton<InventoryUIManager>
{
    [SerializeField] GameObject bagUI;
    [SerializeField] GameObject chestUI;
    [SerializeField] GameObject armorUI;
    [SerializeField] GameObject equippedUI;
    [SerializeField] GameObject itemDescUI;
    [SerializeField] GameObject hud;
    
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
            hud.SetActive(false);
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
            hud.SetActive(false);
            openUI = UI.Bag;
        }
    }

    public void CloseAllUI() {
        bagUI.SetActive(false);
        chestUI.SetActive(false);
        armorUI.SetActive(false);
        equippedUI.SetActive(false);
        hud.SetActive(true);
        openUI = UI.Closed;
    }
}
