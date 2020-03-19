using UnityEngine;

public class Bullet:IAttackFlyMode
{
    private GameObject bulletObjectPrefab;//子弹物体的prefab
    public Vector3 startPosition { get; set; }

    public Bullet(GameObject bulletObjectPrefab, Vector3 startPosition)
    {
        this.bulletObjectPrefab = bulletObjectPrefab;
        this.startPosition = startPosition;
    }

    //动画与弹道同时进行，子弹生成与结算具体由ITarget中实现
    public void AttackFly(ITarget t, IAttackResultMode m, float attackValue)
    {
        PlayAnimation();
        GlobalChangeController.GetInstance().StartCoroutine(t.AttackReach(m, attackValue, bulletObjectPrefab, startPosition));
    }

    //攻击动画
    private void PlayAnimation()
    {

    }
}
