using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

public class LuaBehaviour : MonoBehaviour
{
    private LuaEnv m_LuaEnv = Manager.Lua.LuaEnv; //虚拟机
    protected LuaTable m_ScriptEnv; //运行环境，子类需要用到这个运行环境，所以是protected
    private Action m_LuaInit;
    private Action m_LuaUpdate;
    private Action m_LuaOnDestroy;

    private void Awake()
    {
        m_ScriptEnv = m_LuaEnv.NewTable();
        // 为每个脚本设置一个独立的环境，可一定程度上防止脚本间全局变量、函数冲突
        LuaTable meta = m_LuaEnv.NewTable();
        meta.Set("__index", m_LuaEnv.Global);
        m_ScriptEnv.SetMetaTable(meta);
        meta.Dispose();
        m_ScriptEnv.Set("self", this);  //绑定脚本的对象是lua脚本中的self
    }

    public virtual void Init(string luaName)
    {
        m_LuaEnv.DoString(Manager.Lua.GetLuaScript(luaName), luaName, m_ScriptEnv); //lua脚本赋给c#
        m_ScriptEnv.Get("Update", out m_LuaUpdate); //c#的和lua的绑定
        m_ScriptEnv.Get("OnInit", out m_LuaInit);
        m_LuaInit?.Invoke();
    }


    // Update is called once per frame
    void Update()
    {
        m_LuaUpdate?.Invoke();
    }
    //把运行环境和方法释放掉，子类会重写这个方法
    protected virtual void Clear()
    {
        m_LuaOnDestroy = null;
        m_ScriptEnv?.Dispose();
        m_ScriptEnv = null;
        m_LuaInit = null;
        m_LuaUpdate = null;
    }

    private void OnDestroy()
    {
        m_LuaOnDestroy?.Invoke();
        Clear();
    }
    private void OnApplicationQuit()
    {
        Clear();
    }
}
