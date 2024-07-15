﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILogic : LuaBehaviour
{
    public string AssetName;
    Action m_LuaOnOpen;
    Action m_LuaOnClose;
    public override void Init(string luaName)
    {
        base.Init(luaName);
        m_ScriptEnv.Get("OnOpen", out m_LuaOnOpen); //c#的OnOpen()是lua的m_LuaOnOpen
        m_ScriptEnv.Get("OnClose", out m_LuaOnClose);
    }

    public void OnOpen()
    {
        m_LuaOnOpen?.Invoke();
    }

    public void Close()
    {
        m_LuaOnClose?.Invoke();
        Manager.Pool.UnSpawn("UI", AssetName, this.gameObject); //传的资源是挂载了这个脚本的资源
    }

    protected override void Clear()
    {
        base.Clear();
        m_LuaOnOpen = null;
        m_LuaOnClose = null;
    }
}
