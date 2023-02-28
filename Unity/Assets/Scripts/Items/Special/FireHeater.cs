using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireHeater : Heater
{
    
    public override float Heating => Mathf.Max(TemperatureSystem.the().OutsideTemperature, provideHeat);

    public override string GetItemName() {
        return "FireHeater";
    }

    public override string GetItemDescription() {
        return "The fire is beautiful warm.";
    }

    public override bool IsCollectible() {
        return false;
    }
}
