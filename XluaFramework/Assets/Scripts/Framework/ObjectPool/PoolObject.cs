using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject
{
    //具体对象
    public Object Object;

    //对象名字
    public string Name;

    //最后一次使用时间
    public System.DateTime LastUseTime;
    //对象池：存，取，销毁

    public PoolObject(string name, Object obj)
    {
        Name = name;
        Object = obj;
        LastUseTime = System.DateTime.Now; //构造的时候把当前时间赋值给上次使用时间
    }
}
