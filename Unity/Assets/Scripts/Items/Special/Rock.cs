using UnityEngine;

namespace Items.Special
{
    public class Rock : NonPermanentItem
    {
        public override void UseConcreteItem() {
            Debug.Assert(false);
        }

        public override void UseItem() {
            Debug.Log("Item cannot be used");
        }

        public override string GetItemName() {
            return "Rocks";
        }

        public override string GetItemDescription()
        {
            return "Rocks are useless";
        }
        
        public override bool IsInteractible() {
            return true;
        }

        public override bool IsCollectible() {
            return false;
        }
        
    }
}