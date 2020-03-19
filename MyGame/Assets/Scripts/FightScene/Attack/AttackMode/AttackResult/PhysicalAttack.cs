public class PhysicalAttack:IAttackResultMode
{
    public IFilter filter { get; set; }
    public PhysicalAttack(IFilter filter)
    {
        this.filter = filter;
    }

    //物理攻击，伤害（攻击-防御），不破防-1
    public float getHealthReduction(Creature c, float attackValue)
    {
        if (attackValue - c.defenseValue > 0)
        {
            return (attackValue - c.defenseValue);
        }
        else
        {
            return 1;
        }
    }
}
