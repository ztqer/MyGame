public interface IAttackResultMode
{
    IFilter filter { get; set; }
    float getHealthReduction(Creature c, float attackValue);
}
