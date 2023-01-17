using UnityEngine;
using UnityEngine.EventSystems;

public class UIGameInput : MonoBehaviour {
    public GameObject firstUIObject;

    void Start() {
        if (firstUIObject != null) EventSystem.current.SetSelectedGameObject(firstUIObject);
    }

    void OnCancel() {
        Debug.Log("Cancel");
        InventoryUIManager.the().CloseAllUI();
    }
}
