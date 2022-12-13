public abstract class ArmorItem : PermanentItem
{
    public override bool IsArmor() {
        return true;
    }

    abstract public float TemperatureBuff();
}
