using System.Collections.Generic;

public class SoilderFilter:AbstractFilter
{
    public SoilderFilter(IFilter f):base(f){ }

    //筛选Soilder子集
    public override List<Creature> ConcreteFilter(List<Creature> orignalList, int max)
    {
        return FilterList<Soilder>(orignalList, max);
    }
}
