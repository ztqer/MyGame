using System;
using UnityEngine;

[Serializable]
//存储一波敌人信息
public class Wave
{
    public GameObject enemyPrefab;//怪物类型
    public int enemyNum;//怪物数量
    public float rate;//出怪间隔
}
