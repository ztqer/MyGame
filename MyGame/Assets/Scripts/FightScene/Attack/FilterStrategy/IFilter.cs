using System.Collections.Generic;

public interface IFilter
{
    List<Creature> FilterList(List<Creature> orignalList, int max);
}
