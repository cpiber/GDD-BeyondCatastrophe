using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boots : ArmorItem
{    
    public override void UseItem() {

    }

    public override string GetItemName() {
        return "Boots";
    }

    public override string GetItemDescription() {
        return "The boots keep you warm.";
    }

    public override float TemperatureBuff() {
        return 3f;
    }
}
