public class Healing:IAttackResultMode
{
    public IFilter filter { get; set; }
    public Healing(IFilter filter)
    {
        this.filter = filter;
    }

    //治疗，治疗量=攻击
    public float getHealthReduction(Creature c, float attackValue)
    {
        return -attackValue;
    }
}
