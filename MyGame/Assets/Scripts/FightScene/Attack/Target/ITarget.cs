using System.Collections;
using UnityEngine;

public interface ITarget
{
    IEnumerator AttackReach(IAttackResultMode m, float attackValue, GameObject bulletObjectPrefab, Vector3 startPosition);
}