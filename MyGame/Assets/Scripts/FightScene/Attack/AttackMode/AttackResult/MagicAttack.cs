public class MagicAttack:IAttackResultMode
{
    public IFilter filter { get; set; }
    public MagicAttack(IFilter filter)
    {
        this.filter = filter;
    }

    //魔法攻击，伤害（攻击*（1-魔抗）%），满魔抗-1
    public float getHealthReduction(Creature c, float attackValue)
    {
        if (c.magicResistance >= 100)
        {
            return 1;
        }
        else
        {
            return (attackValue * (100 - c.magicResistance) / 100);
        }
    }
}
