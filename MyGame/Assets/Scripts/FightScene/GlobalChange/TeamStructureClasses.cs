using System;

//存储羁绊信息类
public class TeamStructureStorage:IStorage
{
    //名字与数量
    public PublicData.TeamStructureType name { get; }
    private int num;
    public int Num
    {
        get => num;
        set
        {
            //监听num改变时触发广播
            if (num != value)
            {
                TeamStructureMessage m = new TeamStructureMessage(name, num, value);
                GlobalChangeController.GlobalChangeEventArgs e = new GlobalChangeController.GlobalChangeEventArgs(m);
                GlobalChangeController.GetInstance().OnGlobalChange(e);
            }
            num = value;
        }
    }
    public TeamStructureStorage(PublicData.TeamStructureType t)
    {
        name = t;
        num = 0;
    }
}

//羁绊改变信息（类别与新旧数量）
public class TeamStructureMessage : IMessage
{
    public PublicData.GlobalChangeType changeType{ get; } = PublicData.GlobalChangeType.teamStructure;
    public PublicData.TeamStructureType teamStructureType { get; }
    public int oldNum { get; }
    public int newNum { get; }

    public TeamStructureMessage(PublicData.TeamStructureType t, int oldNum, int newNum)
    {
        teamStructureType = t;
        this.oldNum = oldNum;
        this.newNum = newNum;
    }
}

//羁绊创建工厂
public class TeamStructureStorageFactory : IStorageFactory
{
    public IStorage[] Produce()
    {
        //遍历枚举生成羁绊信息存储类的数组
        TeamStructureStorage[] teams;
        int length = Enum.GetNames(typeof(PublicData.TeamStructureType)).GetLength(0);
        teams = new TeamStructureStorage[length];
        for (int i = 1; i <= length; i++)
        {
            teams[i - 1] = new TeamStructureStorage((PublicData.TeamStructureType)(i - 1));
        }
        return teams;
    }
}
