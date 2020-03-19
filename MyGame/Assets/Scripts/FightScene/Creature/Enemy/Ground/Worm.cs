public class Worm : GroundEnemy
{
    protected override AttackMode GetAttackMode()
    {
        PhysicalAttack p = new PhysicalAttack(DesignFilter());
        Melee m = new Melee();
        return new AttackMode(p, m);
    }
}
