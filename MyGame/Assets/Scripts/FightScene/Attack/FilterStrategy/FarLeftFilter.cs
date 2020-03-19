using System.Collections.Generic;

public class FarLeftFilter : AbstractFilter
{
    public Creature creature;//使用者
    public FarLeftFilter(IFilter f, Creature c) : base(f)
    {
        creature = c;
    }

    //将集合成员按与基地距离（靠左）排序选出指定数量的对象
    public override List<Creature> ConcreteFilter(List<Creature> orignalList, int max)
    {
        float[] distance = new float[orignalList.Count];
        float cursor=0;
        int num = 0;
        for (int i = 1; i <= orignalList.Count; i++)
        {
            distance[i - 1] = orignalList[i - 1].transform.position.z;
            if (distance[i - 1] >= cursor && num<=max)
            {
                cursor = distance[i - 1];
                num++;
            }
        }
        List<Creature> result = new List<Creature>();
        for (int i = 1; i <= orignalList.Count; i++)
        {
            if (distance[i - 1] <= cursor && result.Count<max)
            {
                result.Add(orignalList[i-1]);
            }
        }
        return result;
    }
}