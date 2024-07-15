using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class SceneLogic : LuaBehaviour
{
    public string SceneName;

    Action m_LuaActive;
    Action m_LuaInActive;
    Action m_LuaOnEnter;
    Action m_LuaOnQuit;

    public override void Init(string luaName)
    {
        base.Init(luaName);
        m_ScriptEnv.Get("OnActive", out m_LuaActive); //场景切换时，另一个激活，这个就不激活
        m_ScriptEnv.Get("OnInActive", out m_LuaInActive);
        m_ScriptEnv.Get("OnEnter", out m_LuaOnEnter); //第一次加载
        m_ScriptEnv.Get("OnQuit", out m_LuaOnQuit); //卸载
    }

    public void OnActive()
    {
        m_LuaInActive?.Invoke();
    }

    public void OnInActive()
    {
        m_LuaInActive?.Invoke();
    }

    public void OnEnter()
    {
        m_LuaOnEnter?.Invoke();
    }

    public void OnQuit()
    {
        m_LuaOnQuit?.Invoke();
    }

    protected override void Clear()
    {
        base.Clear();
        m_LuaActive = null;
        m_LuaInActive = null;
        m_LuaOnEnter = null;
        m_LuaOnQuit = null;
    }
}
