using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EntityLogic : LuaBehaviour
{
    Action m_LuaOnShow;
    Action m_LuaOnHide;

    public override void Init(string luaName)
    {
        base.Init(luaName);
        m_ScriptEnv.Get("OnShow", out m_LuaOnShow);
        m_ScriptEnv.Get("OnHide", out m_LuaOnHide);
    }

    public void OnShow()
    {
        m_LuaOnShow?.Invoke();
    }

    public void OnHide()
    {
        m_LuaOnHide?.Invoke(); //关闭不是销毁，还会重复用
    }

    protected override void Clear()
    {
        base.Clear();
        m_LuaOnHide = null;
        m_LuaOnShow = null;
    }
}
