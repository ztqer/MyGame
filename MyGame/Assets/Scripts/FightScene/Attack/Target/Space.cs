using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Space:MonoBehaviour,ITarget
{
    List<Creature> creaturesCanAttack;//范围内的对象

    private void Awake()
    {
        gameObject.GetComponent<Collider>().enabled = false;
    }

    public IEnumerator AttackReach(IAttackResultMode m, float attackValue, GameObject bulletObjectPrefab, Vector3 startPosition)
    {
        creaturesCanAttack = new List<Creature>();
        gameObject.GetComponent<Collider>().enabled = false;
        //生成子弹并设定其目的地
        if (bulletObjectPrefab != null)
        {
            GameObject bulletGameObject = Instantiate(bulletObjectPrefab);
            bulletGameObject.transform.position = startPosition;
            BulletObject bulletObject = bulletGameObject.GetComponent<BulletObject>();
            bulletObject.target = gameObject;
            yield return new WaitUntil(bulletObject.ReachSignal);
        }
        gameObject.GetComponent<Collider>().enabled = true;
        yield return 0;//必须等到下一帧触发器结果出来
        CreatureGroup group = new CreatureGroup(m.filter.FilterList(creaturesCanAttack, PublicData.noMax));
        print("666");
        StartCoroutine(group.AttackReach(m, attackValue, null,PublicData.noMeaning));
    }

    private void OnTriggerEnter(Collider other)
    {
        //忽视other的触发器
        if (other.isTrigger == false)
        {
            //根据layer作按位与判断目标是否为指定对象
            int objectlayermask = 1 << other.gameObject.layer;
            if ((objectlayermask & LayerMask.GetMask(new string[2] { "soilder", "enemy" })) > 0)
            {
                creaturesCanAttack.Add(other.GetComponent<Creature>());
            }
        }
    }
}
