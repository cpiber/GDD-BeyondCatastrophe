using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
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

    public Sprite GetSprite() {
        return sprite;
    }
}
