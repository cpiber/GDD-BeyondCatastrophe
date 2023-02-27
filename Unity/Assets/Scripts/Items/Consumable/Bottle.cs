using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : NonPermanentItem
{
    [SerializeField] float foodValue = 5f;

    public override void UseConcreteItem() {
        StatusSystem.the().Eat(foodValue);
    }

    public override string GetItemName() {
        return "Bottle";
    }

    public override string GetItemDescription() {
        return "Some cheap wine.";
    }
}
