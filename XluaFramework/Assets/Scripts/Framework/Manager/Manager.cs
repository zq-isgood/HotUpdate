﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    //监听者模式
    private static ResourceManager _resource;
    public static ResourceManager Resource
    {
        get { return _resource; }
    }

    private static LuaManager _lua;
    public static LuaManager Lua
    {
        get { return _lua; }
    }

    private static UIManager _ui;
    public static UIManager UI
    {
        get { return _ui; }
    }

    private static EntityManager _entity;
    public static EntityManager Entity
    {
        get { return _entity; }
    }

    private static MySceneManager _scene;
    public static MySceneManager Scene
    {
        get { return _scene; }
    }

    private static SoundManager _sound;
    public static SoundManager Sound
    {
        get { return _sound; }
    }

    private static EventManager _event;
    public static EventManager Event
    {
        get { return _event; }
    }

    private static PoolManager _pool;
    public static PoolManager Pool
    {
        get { return _pool; }
    }

    private static NetManager _net;
    public static NetManager Net
    {
        get { return _net; }
    }

    private void Awake()
    {
        _resource = this.gameObject.AddComponent<ResourceManager>();
        _lua = this.gameObject.AddComponent<LuaManager>();
        _ui = this.gameObject.AddComponent<UIManager>();
        _entity = this.gameObject.AddComponent<EntityManager>();
        _scene = this.gameObject.AddComponent<MySceneManager>();
        _sound = this.gameObject.AddComponent<SoundManager>();
        _event = this.gameObject.AddComponent<EventManager>();
        _pool = this.gameObject.AddComponent<PoolManager>();
        _net = this.gameObject.AddComponent<NetManager>();
    }
}
