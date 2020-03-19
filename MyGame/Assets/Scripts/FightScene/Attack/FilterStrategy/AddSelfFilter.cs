using System.Collections.Generic;

public class AddSelfFilter:AbstractFilter
{
    public Creature creature;//使用者
    public AddSelfFilter(IFilter f, Creature c) : base(f)
    {
        creature = c;
    }

    //筛选时包含自身
    public override List<Creature> ConcreteFilter(List<Creature> orignalList, int max)
    {
        List<Creature> result = new List<Creature>();
        if (creature != null)
        {
            result.Add(creature);
        }
        foreach(Creature c in orignalList)
        {
            result.Add(c);
            if (result.Count == max)
            {
                break;
            }
        }
        return result;
    }
}
