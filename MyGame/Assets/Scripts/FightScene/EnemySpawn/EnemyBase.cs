﻿using System.Collections;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public Wave[] waves;//波次集合
    public float waveRate;//波次间隔
    public int baseID;//编号，传递供生成物体的脚本使用
    public float startTime;//第一波出怪时间

    private void Start()
    {
        StartCoroutine(Spawnenemy());
    }

    //协程,由waves提取信息，管理出怪
    private IEnumerator Spawnenemy()
    {
        yield return new WaitForSeconds(startTime);
        foreach(Wave w in waves)
        {
            for (int i = 1; i <=w.enemyNum; i++)
            {
                Enemy enemy = Instantiate(w.enemyPrefab).GetComponent<Enemy>();
                enemy.bornBase = gameObject;
                yield return new WaitForSeconds(w.rate);
            }
            yield return new WaitForSeconds(waveRate);
        }
    }
}
