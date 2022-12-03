using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bag : PermanentItem
{
    public override void UseItem () {
        // change scene
        Debug.Log("Bag opened");
    }

    public override string GetItemName() {
        return "Bag";
    }

    public override string GetItemDescription() {
        return "A classic bag which can hold items.";
    }
}
