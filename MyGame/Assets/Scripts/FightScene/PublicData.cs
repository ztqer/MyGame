using UnityEngine;

public class PublicData
{
    public enum MapName { longmen }//地图列表
    public enum Difficulty { easy, medium, hard }//难度等级
    public enum AssistantName { tomato, chinaboy, lex, wang }//助手列表
    public enum TargetType { creature, creatureGroup, space}//target类型
    public enum StandOn { ground, platform }//干员布置地形
    public enum Directions { left, right, up, down }//干员方向
    public enum EnergyMode { time, attack, noEnergy }//能量积攒方式
    public const int noMax = 1000;//过滤器无上限时用此值代替
    public const float bulletSpeed = 20f;//远程攻击飞行速度
    public static readonly Vector3 noMeaning=new Vector3(0,0,0);//AttackReach中不起作用的position
    public const float cubeSize = 5f;//单元格大小
    public const float costGrowRate = 1f;//cost自然增长速度
    public const int soilderNum = 5;//商店干员列表大小
    public static readonly int[] experience ={1,2,4,8,16};//各等级经验
    public enum GlobalChangeType { teamStructure }//GlobalChange模式
    public enum TeamStructureType { heieshili, tishenshizhe, huangjinjingshen, youqudenvren, aojiao, putongren,
                                    dian, ha, shui, dongwu, huxi, yingling, kaoge}      //羁绊列表
}
