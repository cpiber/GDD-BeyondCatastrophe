using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : NonPermanentItem
{
    [SerializeField] float foodValue = 10f;

    public override void UseConcreteItem() {
        StatusSystem.the().Eat(foodValue);
    }

    public override string GetItemName() {
        return "Apple";
    }

    public override string GetItemDescription() {
        return "An apple a day keeps the doctor away.";
    }
}
