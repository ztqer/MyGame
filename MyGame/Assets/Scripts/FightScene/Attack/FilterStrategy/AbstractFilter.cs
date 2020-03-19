using System.Collections.Generic;

//装饰器模式，每个子类实现一个功能，自由组合来动态调整功能
public abstract class AbstractFilter:IFilter
{
    //构造器传入基础过滤器
    internal IFilter baseFilter;
    protected AbstractFilter(IFilter f)
    {
        baseFilter = f;
    }

    //外界调用此函数，过滤器模式，从一个List<Creature>筛选子集
    //先调用基础过滤器的无上限筛选，在结果基础上在进行自身的筛选（只有最后一个会有上限数量max）
    public List<Creature> FilterList(List<Creature> orignalList, int max)
    {
        List<Creature> tmpList;
        if (baseFilter != null)
        {
            tmpList=baseFilter.FilterList(orignalList,PublicData.noMax);
            return ConcreteFilter(tmpList, max);
        }
        else
        {
            //保证筛选一次非null
            tmpList = FilterList<Creature>(orignalList,PublicData.noMax);
        }
        return ConcreteFilter(tmpList, max);
    }
    //子类实现
    public abstract List<Creature> ConcreteFilter(List<Creature> orignalList, int max);

    //从攻击范围内筛选出指定类型的非null对象
    protected static List<Creature> FilterList<T>(List<Creature> orignalList, int max)
    {
        List<Creature> newList = new List<Creature>();
        int num = 0;
        foreach (Creature c in orignalList)
        {
            if (c != null&&c is T)
            {
                newList.Add(c);
                num++;
            }
            if (num == max)
            {
                break;
            }
        }
        return newList;
    }
}
