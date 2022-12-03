using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : PermanentItem
{

    public override void UseItem() {

    }

    public override string GetItemName() {
        return "Knife";
    }

    public override string GetItemDescription() {
        return "The knife is a handy item which can be used for several tasks like opening a can.";
    } 
}
