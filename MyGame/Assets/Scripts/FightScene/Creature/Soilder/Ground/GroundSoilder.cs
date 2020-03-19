using UnityEngine;

public abstract class GroundSoilder : Soilder
{
    public int stopnum;//阻挡数
    internal int nowstop;//当前阻挡数

    public override void Start()
    {
        base.Start();
        standOn = PublicData.StandOn.ground;
    }

    public override void Update()
    {
        base.Update();
        if (nowstop < stopnum)
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    //达到阻挡数上线后，让物理引擎忽视该刚体
    private void StopEnemy(GroundEnemy enemy,GroundSoilder soilder)
    {
        if (soilder.nowstop < soilder.stopnum)
        {
            enemy.canMove = false;
            soilder.nowstop++;
        }
        else
        {
            enemy.canMove = true;
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    //碰撞后处理阻挡的委托
    private void OnCollisionEnter(Collision other)
    {
        int objectlayermask = 1 << other.gameObject.layer;
        if ((objectlayermask & LayerMask.GetMask(new string[1] { "enemy" })) > 0)
        {
            GroundEnemy enemy=other.gameObject.GetComponent<GroundEnemy>();
            enemy.stopBy = gameObject.GetComponent<GroundSoilder>();
            StopEnemy(enemy, gameObject.GetComponent<GroundSoilder>());
        }
    }
}
