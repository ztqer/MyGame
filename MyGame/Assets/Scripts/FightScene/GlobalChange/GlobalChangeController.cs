using System;
using UnityEngine;

public class GlobalChangeController :MonoBehaviour
{
    //单例模式
    private static GlobalChangeController instance;
    public static GlobalChangeController GetInstance()=>instance;

    //观察者模式
    //委托事件，将属性变化发送给场上干员
    public delegate void GlobalChangeEventHandler(object sender,GlobalChangeEventArgs e);
    public event GlobalChangeEventHandler GlobalChange;
    public class GlobalChangeEventArgs : EventArgs
    {
        public readonly IMessage message;
        public GlobalChangeEventArgs(IMessage m) => message = m;
    }
    public void OnGlobalChange(GlobalChangeEventArgs e)
    {
        GlobalChange?.Invoke(this, e);
    }

    //羁绊列表
    public TeamStructureStorage[] teams;

    private void Awake()
    {
        instance = this;
        //工厂方法模式，扩展时参照TeamStructureClasses添加相应类
        teams =(TeamStructureStorage[]) new TeamStructureStorageFactory().Produce();
    }
}
