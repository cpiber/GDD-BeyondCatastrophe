using System.Collections;
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
        if (firstUIObject == null) return;
        EventSystem.current.SetSelectedGameObject(firstUIObject);
        UpdateSelected(Vector2.down);
    }

    void OnSubmit() {
        var toFocus = EventSystem.current.currentSelectedGameObject?.GetComponent<FocusOnSubmit>()?.Focus;
        if (toFocus != null) StartCoroutine(FocusNext(toFocus));
    }

    IEnumerator FocusNext(GameObject toFocus) {
        yield return null;
        EventSystem.current.SetSelectedGameObject(toFocus);
    }

    void OnMoveMouse(InputValue mv) {
        if (mv.Get<Vector2>().magnitude > 1) EventSystem.current.SetSelectedGameObject(null);
    }

    void OnNavigate(InputValue mv) {
        if (EventSystem.current.currentSelectedGameObject != null) lastUIObject = EventSystem.current.currentSelectedGameObject;
        UpdateSelected(mv.Get<Vector2>());
    }

    void OnCancel() {
        if (mainBackObject != null && mainBackObject.activeInHierarchy) mainBackObject.GetComponent<Button>().onClick.Invoke();
        else if (creditsBackObject != null && creditsBackObject.activeInHierarchy) creditsBackObject.GetComponent<Button>().onClick.Invoke();
    }

    void UpdateSelected(Vector2 mv) {
        var obj = EventSystem.current.currentSelectedGameObject;
        if (obj == null || !obj.activeSelf || !obj.activeInHierarchy) {
            EventSystem.current.SetSelectedGameObject(FindNextButton(mv));
        }
    }

    GameObject FindNextButton(Vector2 mv) {
        var o = lastUIObject?.GetComponent<Button>()?.FindSelectable(mv)?.gameObject;
        if (o != null) return o;
        return GameObject.FindObjectOfType<Button>().gameObject;
    }
}
