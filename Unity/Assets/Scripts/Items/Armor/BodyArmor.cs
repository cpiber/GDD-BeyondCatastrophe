using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyArmor : ArmorItem
{    
    public override void UseItem() {

    }

    public override string GetItemName() {
        return "BodyArmor";
    }

    public override string GetItemDescription() {
        return "The body armor helps when its cold.";
    }

    public override float TemperatureBuff() {
        return 10f;
    }
}
