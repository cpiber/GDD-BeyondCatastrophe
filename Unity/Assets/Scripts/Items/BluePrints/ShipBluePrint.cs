namespace Items.BluePrints
{
    public class ShipBluePrint : BluePrint
    {
        public override void UseItem() {
            
        }
        
        public override string GetItemName() {
            return "ShipBluePrint";
        }
        
        public override string GetItemDescription() {
            return "This ship blueprint allows you to repair the ship at the port.";
        }
    }
}