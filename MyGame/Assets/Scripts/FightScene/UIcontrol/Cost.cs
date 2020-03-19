using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class Cost : MonoBehaviour
{
    public static int cost;//资源cost
    private Text text1;//cost UI文字

    //设置UI
    private void Start()
    {
        text1 = gameObject.GetComponent<Text>();
        cost= Convert.ToInt32(Regex.Replace(text1.text, "cost:",""));
        StartCoroutine(CostGrow());
    }
    private void Update()
    {
        text1.text = "cost:" + cost;
    }

    //协程,cost随时间增加
    private IEnumerator CostGrow()
    {
        while (true)
        {
            yield return new WaitForSeconds(PublicData.costGrowRate);
            cost++;
        }
    }
}
