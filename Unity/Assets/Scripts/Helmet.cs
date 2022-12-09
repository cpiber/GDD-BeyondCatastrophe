using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helmet : PermanentItem
{

    public override void UseItem() {

    }

    public override string GetItemName() {
        return "Helmet";
    }

    public override string GetItemDescription() {
        return "The helmet increases your defense";
    } 

    public override bool IsArmor() {
        return true;
    }
}
