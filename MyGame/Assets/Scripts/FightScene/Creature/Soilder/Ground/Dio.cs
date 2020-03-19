using System.Collections.Generic;
using UnityEngine;
public class Dio : GroundSoilder
{
    public override void ApplyChange(IMessage m)
    {
        print("change");
    }

    protected override AttackMode GetAttackMode()
    {
        PhysicalAttack p = new PhysicalAttack(DesignFilter());
        Bullet b = new Bullet(Resources.Load("Bullets/bullet")as GameObject,new Vector3(transform.position.x,transform.position.y*1.5f,transform.position.z));
        return new AttackMode(p, b);
    }

    protected override void UseAbility()
    {

    }
}
