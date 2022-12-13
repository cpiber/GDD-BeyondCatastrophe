public abstract class Armor : PermanentItem
{
    public override bool IsArmor() {
        return true;
    }

    abstract public float TemperatureBuff();
}
