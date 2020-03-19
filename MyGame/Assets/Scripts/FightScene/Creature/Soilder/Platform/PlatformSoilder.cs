using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  abstract class PlatformSoilder : Soilder
{

    public override void Start()
    {
        base.Start();
        standOn = PublicData.StandOn.platform;
    }

}
