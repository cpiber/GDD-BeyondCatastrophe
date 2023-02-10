public class Bag : PermanentItem
{
    public override void UseItem () {
        // open chest overlay (chest and inventory)
        InventoryUIManager.the().ToggleBagUI();
    }

    public override string GetItemName() {
        return "Bag";
    }

    public override string GetItemDescription() {
        return "A classic bag which can hold items.";
    }
}
