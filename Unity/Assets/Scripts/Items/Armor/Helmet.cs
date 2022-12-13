using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helmet : ArmorItem
{    
    public override void UseItem() {

    }

    public override string GetItemName() {
        return "Helmet";
    }

    public override string GetItemDescription() {
        return "The helmet increases your defense";
    }

    public override float TemperatureBuff() {
        return 5f;
    }
}
