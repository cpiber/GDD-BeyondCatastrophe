namespace Items.BluePrints
{
    public class ShipBluePrint : BluePrint
    {
        public const string SHIPBLUEPRINT_COLLECTED = "shipBluePrintCollected";
        
        public override void UseItem() {
            
        }
        
        public override string GetItemName() {
            return "ShipBluePrint";
        }
        
        public override string GetItemDescription() {
            return "This ship blueprint allows you to repair the ship at the port.";
        }
        
        public override void OnCollect() {
            ProgressSystem.the().setProgress(SHIPBLUEPRINT_COLLECTED);
        }
    }
}