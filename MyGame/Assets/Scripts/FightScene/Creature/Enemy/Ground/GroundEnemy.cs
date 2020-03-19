using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GroundEnemy : Enemy
{
    internal GroundSoilder stopBy;


    //阻挡者死亡后继续移动
    public override void Update()
    {
        base.Update();
        if (stopBy == false)
        {
            canMove = true;
        }
    }

    //释放占用的阻挡数
    public override void Die()
    {
        base.Die();
        if (stopBy != null)
        {
            stopBy.nowstop--;
        }
    }

}
