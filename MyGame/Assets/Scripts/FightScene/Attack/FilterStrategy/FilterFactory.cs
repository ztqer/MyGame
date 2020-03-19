public static class FilterFactory
{
    //享元模式，将常用的几种过滤器存储于静态工厂中，用户获取时传递引用代替创建
    private static FarLeftFilter normalFilterForSoilder=new FarLeftFilter(new EnemyFilter(null),null);
    private static NearestFilter normalFilterForEnemy=new NearestFilter(new SoilderFilter(null),null);
    private static NearestFilter normalFilterForHealerSoilder=new NearestFilter(new AddSelfFilter(new EnemyFilter(null),null),null);

    public static IFilter getNormalFilterForSoilder(Creature c)
    {
        normalFilterForSoilder.creature = c;
        return normalFilterForSoilder;
    }
    public static IFilter getNormalFilterForEnemy(Creature c)
    {
        normalFilterForEnemy.creature = c;
        return normalFilterForEnemy;
    }
    public static IFilter getNormalFilterForHealerSoilder(Creature c)
    {
        normalFilterForHealerSoilder.creature = c;
        ((AddSelfFilter)normalFilterForHealerSoilder.baseFilter).creature = c;
        return normalFilterForHealerSoilder;
    }
}
