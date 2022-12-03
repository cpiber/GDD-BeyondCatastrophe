using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NonPermanentItem : Item
{
    bool isReusable;
    int numberOfUsagesLeft; 

    public void IncreaseItemCount() {
        numberOfUsagesLeft += 1;
    }

    public override bool IsReusable() {
        return isReusable;
    }

    public int Count() {
        return numberOfUsagesLeft;
    }
}
