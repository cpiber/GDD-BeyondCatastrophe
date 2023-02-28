namespace Items.BluePrints
{
    public class ShipBluePrint : BluePrint
    {
        public const string SHIPBLUEPRINT_COLLECTED_PORT = "shipBluePrintCollected_PORT";
        public const string SHIPBLUEPRINT_COLLECTED_DI = "shipBluePrintCollected_DI";
        public override void UseItem() {
            
        }
        
        public override string GetItemName() {
            return "ShipBluePrint";
        }
        
        public override string GetItemDescription() {
            return "This ship blueprint allows you to repair the ship at the port.";
        }
        
        public override void OnCollect() {
            if (this.name == "ShipBluePrintDI")
            {
                ProgressSystem.the().setProgress(SHIPBLUEPRINT_COLLECTED_DI);
            }
            
            if (this.name == "ShipBluePrintPort")
            {
                ProgressSystem.the().setProgress(SHIPBLUEPRINT_COLLECTED_PORT);
            }
        }
    }
}