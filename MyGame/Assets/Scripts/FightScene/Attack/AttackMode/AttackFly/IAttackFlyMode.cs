using UnityEngine;

public interface IAttackFlyMode
{
    Vector3 startPosition { get; set; }
    void AttackFly(ITarget t, IAttackResultMode m, float attackValue);
}
