using UnityEngine;

public class Melee:IAttackFlyMode
{
    public Vector3 startPosition { get; set; }
    //动画结束后进行结算
    public void AttackFly(ITarget t, IAttackResultMode m, float attackValue)
    {
        PlayAnimation();
        GlobalChangeController.GetInstance().StartCoroutine(t.AttackReach(m, attackValue, null,startPosition));
    }

    //攻击动画
    private void PlayAnimation()
    {

    }
}
