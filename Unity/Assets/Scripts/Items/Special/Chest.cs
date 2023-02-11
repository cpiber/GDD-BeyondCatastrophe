public class Chest : PermanentItem
{
    public override void UseItem () {
        // open chest overlay (chest and inventory)
        InventoryManager.the().UseChest();
    }

    public override string GetItemName() {
        return "Chest";
    }

    public override string GetItemDescription() {
        return "A classic chest which can hold items.";
    }

    public override bool IsInteractible() {
        return true;
    }

    public override bool IsCollectible() {
        return false;
    }
}
