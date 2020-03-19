using System.Collections.Generic;

public class EnemyFilter : AbstractFilter
{
    public EnemyFilter(IFilter f) : base(f){ }

    //筛选Enemy子集
    public override List<Creature> ConcreteFilter(List<Creature> orignalList, int max)
    {
        return FilterList<Enemy>(orignalList, max);
    }
}
