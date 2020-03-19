using System.Collections.Generic;
using UnityEngine;

public class NearestFilter:AbstractFilter
{
    public Creature creature;//使用者
    public NearestFilter(IFilter f,Creature c) : base(f)
    {
        creature = c;
    }

    //将集合成员按自身与其距离排序选出指定数量的对象
    public override List<Creature> ConcreteFilter(List<Creature> orignalList, int max)
    {
        float[] distanceSquare = new float[orignalList.Count];
        float cursor = 0;
        int num = 0;
        for (int i = 1; i <= orignalList.Count; i++)
        {
            distanceSquare[i - 1] = Mathf.Pow(orignalList[i - 1].transform.position.x - creature.transform.position.x, 2) + Mathf.Pow(orignalList[i - 1].transform.position.z - creature.transform.position.z, 2);
            if (distanceSquare[i - 1] >= cursor && num <= max)
            {
                cursor = distanceSquare[i - 1];
                num++;
            }
        }
        List<Creature> result = new List<Creature>();
        for (int i = 1; i <= orignalList.Count; i++)
        {
            if (distanceSquare[i - 1] <= cursor && result.Count < max)
            {
                result.Add(orignalList[i - 1]);
            }
        }
        return result;
    }
}
