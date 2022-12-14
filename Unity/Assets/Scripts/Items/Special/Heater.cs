using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heater : PermanentItem
{
    [SerializeField] float provideHeat;
    /// <summary>
    /// The heating this item provides. Taken to be a temperature for now.
    /// </summary>
    public float Heating => provideHeat;

    public override void UseItem () {
        // TODO heating UI?
    }

    public override string GetItemName() {
        return "Heater";
    }

    public override string GetItemDescription() {
        return "A heating device used to heat up a room.";
    }

    public override bool IsCollectible() {
        return false;
    }
}
