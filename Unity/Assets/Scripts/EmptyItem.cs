using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyItem : Item
{

    public override void UseItem() {

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
        return false;
    }
}
