using UnityEngine;
using UnityEngine.UI;

public class Assistant : MonoBehaviour
{
    private PublicData.AssistantName assistant;

    private void Start()
    {
        //读取设置，布置图像
        assistant = GameSet.gameSet.userSet.assistantName;
        switch (assistant)
        {
            case PublicData.AssistantName.tomato:
                transform.GetComponent<Image>().sprite = Resources.Load("Assistants/tomato",typeof(Sprite)) as Sprite;
                break;
            case PublicData.AssistantName.chinaboy:
                transform.GetComponent<Image>().sprite = Resources.Load("Assistants/chinaboy", typeof(Sprite)) as Sprite;
                break;
            case PublicData.AssistantName.lex:
                transform.GetComponent<Image>().sprite = Resources.Load("Assistants/lex", typeof(Sprite)) as Sprite;
                break;
            case PublicData.AssistantName.wang:
                transform.GetComponent<Image>().sprite = Resources.Load("Assistants/wang", typeof(Sprite)) as Sprite;
                break;
        }
    }
}
