public interface ICanAttack
{
    ITarget ChooseTarget(Space space,int targetNum);
    void Attack(AttackMode m, ITarget t, float attackValue);
}
