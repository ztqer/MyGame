using System;
//建造者模式，扩展时在下列类中添加属性并在此处添加设置方法
public class GameSetBuilder
{
    private GameSet gameSet=new GameSet();
    private UserSet userSet=new UserSet();
    private MapSet mapSet=new MapSet();
    private DiffficultySet diffficultySet=new DiffficultySet();

    public void ReadChoices(string name,PublicData.AssistantName aName,
        PublicData.Difficulty d=PublicData.Difficulty.easy,PublicData.MapName mName=PublicData.MapName.longmen)
    {
        userSet.userName = name;
        userSet.assistantName = aName;
        mapSet.mapName = mName;
        diffficultySet.difficulty = d;
    }

    public GameSet Build()
    {
        gameSet.userSet = userSet;
        gameSet.mapSet = mapSet;
        gameSet.diffficultySet = diffficultySet;
        return gameSet;
    }
}
