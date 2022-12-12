using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : NonPermanentItem
{
    public override void UseConcreteItem() {
        Debug.Log("Apple is now used");
    }

    public override string GetItemName() {
        return "Apple";
    }

    public override string GetItemDescription() {
        return "An apple a day keeps the doctor away.";
    }
}

// inventory manager
// => alle items
// => 