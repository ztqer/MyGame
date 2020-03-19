//桥接模式+状态模式
public class AttackMode
{
    //攻击模式两维度独立变化，各自由子类实现多态
    //根据状态可以动态更换攻击方式与效果（比如技能）
    private IAttackResultMode attackResultMode;
    private IAttackFlyMode attackFlyMode;
    public AttackMode(IAttackResultMode m1, IAttackFlyMode m2)
    {
        attackResultMode = m1;
        attackFlyMode = m2;
    }

    public void AttackFly(ITarget t,float attackValue)
    {
        attackFlyMode.AttackFly(t,attackResultMode,attackValue);
    }
}
