using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NonPermanentItem : Item
{
    [SerializeField] int numberOfUsagesLeft; 

    public abstract void UseConcreteItem();
    
    public void IncreaseItemCount(NonPermanentItem other = null) {
        numberOfUsagesLeft += other?.numberOfUsagesLeft ?? 1;
    }

    public void SetUsages(NonPermanentItem other = null) {
        numberOfUsagesLeft = other?.numberOfUsagesLeft ?? 1;
    }

    public override bool IsItemEmpty() {
        return false;
    }

    public override bool IsReusable() {
        return false;
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


    public bool DecreaseItemCount(int number = 1) {
        if (numberOfUsagesLeft >= number) {
            numberOfUsagesLeft -= number;
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
