using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public const float OUTLINE_GROW = .1f;

    Location location;

    enum Location {
        Inventory,
        Bag,
        Equipped
    };

    Location GetLocation() {
        return location;
    }

    abstract public void UseItem ();
    abstract public string GetItemName();
    abstract public bool IsReusable();
    abstract public bool IsItemEmpty();
    virtual public bool IsArmor() => false;
    virtual public bool IsInteractible() => false;
    virtual public bool IsCollectible() => true;
    abstract public string GetItemDescription();
    [SerializeField] Sprite sprite;
    [SerializeField] Material outlineMaterial;
    [SerializeField] [HideInInspector] protected GameObject outlineObject;
    public GameObject OutlineObject => outlineObject;

    public Sprite GetSprite() {
        return sprite;
    }

    void Start() {
        SetSprite();
    }

    [ContextMenu("Set Sprite")]
    protected void SetSprite() {
        if (outlineMaterial == null) return;
        var renderer = GetComponent<SpriteRenderer>();
        if (renderer == null) return;
        
        var nObject = new GameObject("Outline Helper");
        nObject.transform.parent = transform;
        var nRenderer = nObject.AddComponent<SpriteRenderer>();
        nRenderer.sprite = renderer.sprite;
        nRenderer.material = outlineMaterial;
        nRenderer.sortingOrder = renderer.sortingOrder - 1;
        nRenderer.color = PlayerController.the().ItemOutlineColor;
        nObject.transform.localPosition = Vector3.zero;
        nObject.transform.localScale += new Vector3(OUTLINE_GROW, OUTLINE_GROW, 0);
        nObject.SetActive(false);
        outlineObject = nObject;
    }
}
