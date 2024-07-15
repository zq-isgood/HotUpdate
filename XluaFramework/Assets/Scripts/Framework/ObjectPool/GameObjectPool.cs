using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool : PoolBase
{
    public override Object Spwan(string name)
    {
        Object obj = base.Spwan(name);
        if (obj == null)
            return null;

        GameObject go = obj as GameObject;
        go.SetActive(true); //如果在，就显示
        return obj;
    }

    public override void UnSpwan(string name, Object obj)
    {
        GameObject go = obj as GameObject;
        go.SetActive(false);
        go.transform.SetParent(this.transform, false); //挂载的父节点改变，被回收
        base.UnSpwan(name, obj); //把对象放进池子中
    }

    public override void Release()
    {
        base.Release();
        foreach (PoolObject item in m_Objects)
        {
            if (System.DateTime.Now.Ticks - item.LastUseTime.Ticks >= m_ReleaseTime * 10000000)
            {
                //销毁对象
                Debug.Log("GameObjectPool release  time:" + System.DateTime.Now);
                Destroy(item.Object);
                Manager.Resource.MinusBundleCount(item.Name);
                m_Objects.Remove(item);
                Release(); //递归，因为移除后再循环会出问题
                return;
            }
        }
    }
}
