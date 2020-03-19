using UnityEngine;

public abstract class Enemy : Creature,IEnemy
{
    private Transform[]route;//路线
    private int moveIndex ;//当前目标路径点
    internal GameObject bornBase;//出生点
    public float speed;//移动速度
    internal bool canMove=true;//控制移动

    public virtual void Start()
    {
        Born();
    }

    public override void Update()
    {
        base.Update();
        if (canMove)
        {
            Move();
        }
    }

    //根据出生点绑定路线并设置初始位置
    public override void Born()
    {
        base.Born();
        route = Route.routes[bornBase.GetComponent<EnemyBase>().baseID];
        transform.position = new Vector3(bornBase.transform.position.x, transform.position.y, bornBase.transform.position.z);
    }

    //被击毁
    public override void Die()
    {
        Destroy(gameObject);
    }

    //沿路径点移动
    public void Move()
    {
        if (moveIndex < route.Length)
        {
            transform.Translate((route[moveIndex].position - transform.position).normalized * Time.deltaTime * speed, UnityEngine.Space.World);
            if (Vector3.Distance(route[moveIndex].position, transform.position) < 0.01f)
            {
                moveIndex++;
            }
        }
        else
        {
            Reach();
        }
    }

    //到达销毁及其他处理
    private void Reach()
    {
        Destroy(gameObject);
    }

    //通常的逻辑（最近的Soilder），特殊情况重写此函数
    public override IFilter DesignFilter()
    {
        return FilterFactory.getNormalFilterForEnemy(this);
    }
}
