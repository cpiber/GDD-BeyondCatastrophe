using UnityEngine;

namespace Items.Special
{
    public class WoodLogs : NonPermanentItem
    {
        public override void UseConcreteItem() {
            Debug.Assert(false);
        }

        public override void UseItem() {
            Debug.Log("Item cannot be used");
        }

        public override string GetItemName() {
            return "WoodLog";
        }

        public override string GetItemDescription()
        {
            return "You can use wood logs to repair the boat.";
        }
        
        public override bool IsInteractible() {
            return true;
        }

        public override bool IsCollectible() {
            return false;
        }
        
    }
}