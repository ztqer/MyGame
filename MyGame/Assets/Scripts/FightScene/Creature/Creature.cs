using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Creature : MonoBehaviour, ICreature, ITarget, ICanAttack
{
    public float health;//生命值
    [SerializeField]protected float nowHealth;//当前生命
    public float attackValue;//攻击力
    public float defenseValue;//防御力
    public float magicResistance;//魔法抗性
    public float attackRate;//攻击间隔
    protected AttackMode attackMode;//攻击模式
    protected ITarget target;//目标
    public GameObject attackSpaceObjectPrefab;//attackSpace挂载的物体，借此可以在prefab中可以选择
    protected Space attackSpace;//null表示攻击单个或多个目标,非null则攻击固定范围
    public int targetNum;//目标数量
    protected List<Creature> creaturesCanAttack = new List<Creature>();//攻击范围内的creature集合
    public PublicData.EnergyMode energyMode;//能量模式 0:随时间增长 1:随攻击增长 2:无能量
    public float energyBar;//能量条上限
    public float nowEnergy;//初始能量（设计）或当前能量（游戏中）

    public virtual void Update()
    {
        //判断死亡与刷新血条
        if (nowHealth <= 0)
        {
            nowHealth = 0;
            Die();
        }
        transform.GetChild(0).GetChild(0).GetComponent<Slider>().value = nowHealth / health;
        //时间型能量增长
        if (energyMode == PublicData.EnergyMode.time)
        {
            if (nowEnergy >= energyBar)
            {
                UseAbility();
                nowEnergy = 0;
            }
            else
            {
                nowEnergy += Time.deltaTime / 2;
            }
        }
    }

    //出生
    public virtual void Born()
    {
        attackMode = GetAttackMode();
        if (attackSpaceObjectPrefab != null)
        {
            GameObject attackSpaceObject = Instantiate(attackSpaceObjectPrefab);
            attackSpaceObject.transform.position += transform.position;
            attackSpaceObject.transform.SetParent(transform);
            attackSpace =attackSpaceObject.GetComponent<Space>();
        }
        nowHealth = health;
        StartCoroutine(TryAttack());
    }

    //死亡
    public abstract void Die();

    //初始化攻击模式，由子类实现
    protected abstract AttackMode GetAttackMode();

    //判断攻击目标
    public virtual ITarget ChooseTarget(Space space, int targetNum)
    {
        IFilter filter = DesignFilter();
        List <Creature> result = filter.FilterList(creaturesCanAttack, targetNum);
        if (result.Count == 0)
        {
            return null;
        }
        if (space != null)
        {
            return space;
        }
        if (targetNum == 1)
        {
            return result[0];
        }
        return new CreatureGroup(result);
    }

    //由子类具体决定筛选规则
    public abstract IFilter DesignFilter();

    //发出攻击
    public void Attack(AttackMode m, ITarget t, float attackValue)
    {
        m.AttackFly(target,attackValue);
    }

    //开启协程，每隔attackRate准备好一次攻击，选择target，若有目标则立即发出
    public IEnumerator TryAttack()
    {
        while (true)
        {
            target = ChooseTarget(attackSpace,targetNum);
            if (target==null)
            {
                yield return 0;
                continue;
            }
            //攻击型能量增长
            if (energyMode == PublicData.EnergyMode.attack)
            {
                if (nowEnergy >= energyBar)
                {
                    UseAbility();
                    nowEnergy = 0;
                }
                else
                {
                    nowEnergy++;
                }
            }
            Attack(attackMode, target, attackValue);
            yield return new WaitForSeconds(attackRate);
        }
    }

    //开启协程等待攻击到达
    public IEnumerator AttackReach(IAttackResultMode m, float attackValue, GameObject bulletObjectPrefab, Vector3 startPosition)
    {
        //生成子弹并设定其目标
        if (bulletObjectPrefab != null)
        {
            GameObject bulletGameObject = Instantiate(bulletObjectPrefab);
            bulletGameObject.transform.position=startPosition;
            BulletObject bulletObject = bulletGameObject.GetComponent<BulletObject>();
            bulletObject.target = gameObject;
            if (bulletObject.spaceObjectPrefab != null)
            {
                bulletObject.attackResultMode = m;
                bulletObject.attackValue = attackValue;
                yield break;//终止协程，因为bulletObject中会完成攻击，防止计算2次伤害
            }
            yield return new WaitUntil(bulletObject.ReachSignal);
        }
        //处理攻击结果（各种ITarget的AttackReach最终都会来到这里）
        nowHealth-=m.getHealthReduction(this, attackValue);
    }

    //通过触发器识别碰撞器的进出，添加和移除可攻击对象
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
    private void OnTriggerExit(Collider other)
    {
        if (other.isTrigger == false)
        {
            int objectlayermask = 1 << other.gameObject.layer;
            if ((objectlayermask & LayerMask.GetMask(new string[2] { "soilder", "enemy" })) > 0)
            {
                creaturesCanAttack.Remove(other.GetComponent<Creature>());
            }
        }
    }

    //释放技能,在最终子类中重写
    protected virtual void UseAbility() { }
}
