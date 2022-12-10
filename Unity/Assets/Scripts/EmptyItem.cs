using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyItem : Item
{

    public override void UseItem() {
        Debug.Log("No item equipped");
    }

    public override string GetItemName() {
        return "No item";
    }

    public override string GetItemDescription() {
        return "No item";
    } 

    public override bool IsItemEmpty() {
        return true;
    }

    public override bool IsReusable() {
        return true;
    }

    public override bool IsArmor() {
        return true;
    }
}
