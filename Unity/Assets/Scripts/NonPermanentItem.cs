using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NonPermanentItem : Item
{
    bool isReusable;
    [SerializeField] int numberOfUsagesLeft; 

    public abstract void UseConcreteItem();
    
    public void IncreaseItemCount() {
        numberOfUsagesLeft += 1;
    }

    public override bool IsItemEmpty() {
        return false;
    }

    public override bool IsReusable() {
        return isReusable;
    }

    public override void UseItem() {
        if (CanBeUsed()) {
            DecreaseItemCount();
            UseConcreteItem();
        } else {
            Debug.Log("Item could not be used because there is no item left");
        }
    }

    public bool CanBeUsed() {
        if (numberOfUsagesLeft > 0) {
            return true;
        }
        return false;
    }


    public bool DecreaseItemCount() {
        if (numberOfUsagesLeft > 0) {
            numberOfUsagesLeft -= 1;
            return true;
        }
        else {
            return false;
        }
    }

    public int Count() {
        return numberOfUsagesLeft;
    }
}
