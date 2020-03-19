using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//组合模式，攻击者对单目标和多目标的操作一致
//维护一个List<Creature>
public class CreatureGroup:ITarget
{
    private List<Creature> group;

    public CreatureGroup(List<Creature> l)
    {
        group = l;
    }

    //对每个成员分别开启协程
    public IEnumerator AttackReach(IAttackResultMode m, float attackValue, GameObject bulletObjectPrefab, Vector3 startPosition)
    {
        foreach(Creature c in group)
        {
            c.StartCoroutine(c.AttackReach(m, attackValue, bulletObjectPrefab, startPosition));
        }
        yield break;
    }
}
