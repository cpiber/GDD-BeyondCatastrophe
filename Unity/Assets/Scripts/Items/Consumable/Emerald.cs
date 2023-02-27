using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emerald : NonPermanentItem
{
    [SerializeField] float foodValue = -100000f;

    public override void UseConcreteItem() {
        // TODO: Maybe kill player on pickup
        StatusSystem.the().Eat(foodValue);
    }

    public override string GetItemName() {
        return "Emerald";
    }

    public override string GetItemDescription() {
        return "A misterious Emerald.";
    }
}
