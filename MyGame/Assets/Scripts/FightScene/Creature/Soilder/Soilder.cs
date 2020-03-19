using UnityEngine;
using UnityEngine.UI;

public abstract class Soilder : Creature,ICareAboutGlobalChange
{
    protected PublicData.StandOn standOn;//放置地形 0：地面  1：高台
    internal PublicData.Directions direction = PublicData.Directions.right;//方向
    public GameObject image;//商店UI图片
    public int cost;//费用
    public PublicData.TeamStructureType[] teamStructureTypes;//羁绊类型
    //入场标志，便于放置过程中不激活战斗逻辑
    internal bool isSetOver = false;
    private bool hasBegun;
    public GlobalChangeController globalChangeController;//观察目标

    public virtual void Start()
    {
        globalChangeController = GlobalChangeController.GetInstance();
    }

    //刷新能量条
    public override void Update()
    {
        //拖拽设置完成后，初始化战斗信息
        if (isSetOver == true && hasBegun == false)
        {
            Born();
            //对GlobalChange注册观察者并更新羁绊列表
            globalChangeController.GlobalChange += SelectChange;
            foreach (var type in teamStructureTypes)
            {
                for (int i = 1; i <= globalChangeController.teams.Length; i++)
                {
                    if (type == globalChangeController.teams[i - 1].name)
                    {
                        globalChangeController.teams[i - 1].Num++;
                    }
                }
            }
            hasBegun = true;
        }
        //上场后，开始正常的Update检测
        if (hasBegun == true)
        {
            base.Update();
            if (energyMode == PublicData.EnergyMode.noEnergy)
            {
                Destroy(transform.GetChild(0).GetChild(1).gameObject);
            }
            else
            {
                transform.GetChild(0).GetChild(1).GetComponent<Slider>().value = nowEnergy / energyBar;
            }
        }
    }

    //初始化方向与属性
    public override void Born()
    {
        base.Born();
        switch (direction)
        {
            case PublicData.Directions.left:
                transform.Rotate(new Vector3(0, 180, 0), UnityEngine.Space.Self);
                transform.GetChild(0).transform.Rotate(new Vector3(0, -180, 0), UnityEngine.Space.World);
                break;
            case PublicData.Directions.right:
                break;
            case PublicData.Directions.up:
                transform.Rotate(new Vector3(0, 270, 0), UnityEngine.Space.Self);
                transform.GetChild(0).transform.Rotate(new Vector3(0, -270, 0), UnityEngine.Space.World);
                break;
            case PublicData.Directions.down:
                transform.Rotate(new Vector3(0, 90, 0), UnityEngine.Space.Self);
                transform.GetChild(0).transform.Rotate(new Vector3(0, -90, 0), UnityEngine.Space.World);
                break;
        }
    }

    //被击毁与重生
    public override void Die()
    {
        UIcontroller uic = GameObject.Find("UIcontroller").GetComponent<UIcontroller>();
        for (int i = 1; i <= uic.shop.soilders.Length; i++)
        {
            if (uic.shop.soilders[i - 1] == gameObject)
            {
                uic.whichIsChanged = i;
            }
        }
        uic.Refresh(true);
        Destroy(gameObject);
        uic.activeSoilders--;
        globalChangeController.GlobalChange -= SelectChange;
        foreach (var type in teamStructureTypes)
        {
            for (int i = 1; i <= globalChangeController.teams.Length; i++)
            {
                if (type == globalChangeController.teams[i - 1].name)
                {
                    globalChangeController.teams[i - 1].Num--;
                }
            }
        }
    }

    //通常的逻辑（靠近基地的Enemy），特殊情况重写此函数
    public override IFilter DesignFilter()
    {
        return FilterFactory.getNormalFilterForSoilder(this);
    }

    //筛选羁绊信息并由子类实现具体变化
    public void SelectChange(object sender, GlobalChangeController.GlobalChangeEventArgs e)
    {
        if (e.message.changeType == PublicData.GlobalChangeType.teamStructure)
        {
            ApplyChange((TeamStructureMessage)e.message);
        }
    }
    public abstract void ApplyChange(IMessage m);
}
