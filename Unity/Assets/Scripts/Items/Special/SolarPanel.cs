using UnityEngine;

public class SolarPanel : NonPermanentItem
{
    public override void UseConcreteItem() {
        Debug.Assert(false);
    }

    public override void UseItem() {
        Debug.Log("Item cannot be used");
    }

    public override string GetItemName() {
        return "Solar Panel";
    }

    public override string GetItemDescription() {
        return "No more energy problems.";
    }
}
