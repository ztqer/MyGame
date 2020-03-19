using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public static int level = 1;//等级
    private int nowExperience;//当前经验
    private Text text1;//等级 UI文字
    internal GameObject[] soilders=new GameObject[PublicData.soilderNum];//存放可选的干员prefab
    internal GameObject[] images = new GameObject[PublicData.soilderNum];//相应UI
    internal Vector2 size;//存储imageprefab宽高

   private void Start() 
    {
        text1 = GameObject.Find("UICanvas").transform.GetChild(4).GetComponent<Text>();
        Refresh();
        Load();
    }

    //刷新等级和经验条UI
    private void Update()
    {
        if (nowExperience >= PublicData.experience[level])
        {
            nowExperience -= PublicData.experience[level];
            level++;
        }
        text1.text = "Level:" + level;
        GameObject.Find("NowExperience").GetComponent<Slider>().value =(float)nowExperience / (float)PublicData.experience[level];
    }

    //购买经验(绑定button)
    public void BuyExperience()
    {
        if (Cost.cost >= 10)
        {
            Cost.cost -= 10;
            nowExperience += 1;
        }
    }

    //生成商店干员列表的UI
    private void Load()
    {
        for (int i = 1; i <= soilders.Length; i++)
        {
            images[i - 1] = Instantiate(Resources.Load("UI/Soilders'Images/"+soilders[i - 1].GetComponent<Soilder>().image.name) )as GameObject;
            images[i - 1].transform.SetParent(GameObject.Find("UICanvas").transform);
            if (i > 1)
            {
                images[i - 1].transform.position = new Vector2(images[i - 2].transform.position.x + size.x * 1.1f, images[i - 2].transform.position.y);
            }
            else
            {
                size = images[i - 1].GetComponent<RectTransform>().rect.size;
            }
        }
    }

    //刷新商店干员列表
    private void Refresh()
    {
        for (int i=1;i<=soilders.Length;i++)
        {
            soilders[i-1] = Resources.Load("Soilders/dio") as GameObject;
            if(!soilders[i - 1])
            {
                print(Resources.Load("Soilders/dio"));
            }
        }
    }
}
