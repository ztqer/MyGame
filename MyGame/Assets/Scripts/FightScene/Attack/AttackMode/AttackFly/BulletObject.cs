using UnityEngine;

//适用于远程攻击
public class BulletObject :MonoBehaviour
{
    internal GameObject target;//Creature或Space挂载的物体，决定移动和到达

    //如果spaceObjectPrefab不为null，则实现溅射攻击效果，到达后造成一次范围攻击
    public GameObject spaceObjectPrefab;
    internal IAttackResultMode attackResultMode;
    internal float attackValue;

    //控制信号，控制协程AttackReach执行
    private bool isReached;
    public bool ReachSignal()
    {
        return isReached;
    }

    private void Update()
    {
        Fly();
    }

    private void Fly()
    {
        //飞行中目标已销毁，销毁子弹，相应的攻击协程因为挂载的物体被销毁自动取消
        if (!target)
        {
            Destroy(gameObject);
            isReached = true;
        }
        //向目标移动
        else
        {
            transform.Translate((target.transform.position - transform.position).normalized * Time.deltaTime * PublicData.bulletSpeed, UnityEngine.Space.World);
        }
        if (Vector3.Distance(target.transform.position, transform.position) < PublicData.cubeSize*0.3f)
        {
            isReached = true;
            if (spaceObjectPrefab != null)
            {
                GameObject attackSpaceObject = Instantiate(spaceObjectPrefab);
                attackSpaceObject.transform.position += target.transform.position;
                attackSpaceObject.transform.SetParent(GameObject.Find("AttackSpace").transform);
                Space attackSpace = attackSpaceObject.GetComponent<Space>();
                attackSpace.StartCoroutine(attackSpace.AttackReach(attackResultMode, attackValue, null, PublicData.noMeaning));
            }
            Destroy(gameObject);
        }
    }
}
