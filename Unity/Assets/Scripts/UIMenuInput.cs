using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIMenuInput : MonoBehaviour {
    public GameObject firstUIObject;
    public GameObject mainBackObject;
    public GameObject creditsBackObject;
    private GameObject lastUIObject = null;

    void Start() {
        EventSystem.current.SetSelectedGameObject(firstUIObject);
        UpdateSelected();
    }

    void OnMoveMouse(InputValue mv) {
        EventSystem.current.SetSelectedGameObject(null);
    }

    void OnNavigate(InputValue mv) {
        if (EventSystem.current.currentSelectedGameObject != null) lastUIObject = EventSystem.current.currentSelectedGameObject;
        UpdateSelected();
    }

    void OnCancel() {
        if (mainBackObject.activeInHierarchy) mainBackObject.GetComponent<Button>().onClick.Invoke();
        else if (creditsBackObject.activeInHierarchy) creditsBackObject.GetComponent<Button>().onClick.Invoke();
    }

    void UpdateSelected() {
        var obj = EventSystem.current.currentSelectedGameObject;
        if (obj == null || !obj.activeSelf || !obj.activeInHierarchy) {
            obj = FindNextButton();
            EventSystem.current.SetSelectedGameObject(obj);
        }
    }

    GameObject FindNextButton() {
        var o = lastUIObject?.GetComponent<Button>()?.FindSelectableOnDown()?.gameObject;
        if (o != null) return o;
        return GameObject.FindObjectOfType<Button>().gameObject;
    }
}
