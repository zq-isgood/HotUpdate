﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UObject = UnityEngine.Object;

public class ResourceManager : MonoBehaviour
{


    internal class BundleInfo
    {
        public string AssetsName;
        public string BundleName;
        public List<string> Dependences;
    }
    internal class BundleData
    {
        public AssetBundle Bundle;

        //引用计数，保证引用bundle的对象都被释放后，bundle才可以释放
        public int Count;

        public BundleData(AssetBundle ab)
        {
            Bundle = ab;
            Count = 1;
        }
    }



    //存放Bundle信息的集合，key是资源路径
    private Dictionary<string, BundleInfo> m_BundleInfos = new Dictionary<string, BundleInfo>();
    //存放Bundle资源的集合
    private Dictionary<string, BundleData> m_AssetBundles = new Dictionary<string, BundleData>();
    /// <summary>
    /// 解析版本文件
    /// </summary>
    public void ParseVersionFile()
    {
        //版本文件的路径 filelist.txt
        string url = Path.Combine(PathUtil.BundleResourcePath, AppConst.FileListName);
        string[] data = File.ReadAllLines(url);

        //解析文件信息
        for (int i = 0; i < data.Length; i++)
        {
            BundleInfo bundleInfo = new BundleInfo();
            string[] info = data[i].Split('|');
            bundleInfo.AssetsName = info[0];
            bundleInfo.BundleName = info[1];
            //list特性：本质是数组，但可动态扩容
            bundleInfo.Dependences = new List<string>(info.Length - 2);
            for (int j = 2; j < info.Length; j++)
            {
                bundleInfo.Dependences.Add(info[j]);
            }
            m_BundleInfos.Add(bundleInfo.AssetsName, bundleInfo);

            if (info[0].IndexOf("LuaScripts") > 0) //如果文件路径中有LuaScripts，说明是lua文件
                Manager.Lua.LuaNames.Add(info[0]); //存储lua名
        }
    }

    /// <summary>
    /// 异步加载资源
    /// </summary>
    /// <param name="assetName">资源名</param>
    /// <param name="action">完成回调</param>
    /// <returns></returns>
    IEnumerator LoadBundleAsync(string assetName,Action<UObject> action = null)
    {
       // 拿到资源的bundle名，bundle路径，依赖
        string bundleName = m_BundleInfos[assetName].BundleName;
        string bundlePath = Path.Combine(PathUtil.BundleResourcePath, bundleName);
        List<string> dependences = m_BundleInfos[assetName].Dependences;

        BundleData bundle = GetBundle(bundleName);
        if (bundle == null)
        {
            UObject obj = Manager.Pool.Spawn("AssetBundle", bundleName); //bundle找不到的话，从对象池取出对象
            if (obj != null)
            {
                AssetBundle ab = obj as AssetBundle; //说明对象池里有bundle
                bundle = new BundleData(ab);
            }
            else
            {
                //加载bundle路径，，因为是递归的，所以每个依赖的bundle也会因为这句被加载                
                AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(bundlePath);
                yield return request;
                bundle = new BundleData(request.assetBundle);
            }
            m_AssetBundles.Add(bundleName, bundle);
        }

        if (dependences != null && dependences.Count > 0)
        {
            for (int i = 0; i < dependences.Count; i++)
            {
                yield return LoadBundleAsync(dependences[i]); //递归执行加载
            }
        }
        if (assetName.EndsWith(".unity"))
        {
            action?.Invoke(null);//如果加载的是场景资源，直接返回回调，回调是LoadScene,ChangeScene,没有使用obj
            yield break;
        }

        if (action == null)
        {
            yield break;
        }
        //加载资源
        AssetBundleRequest bundleRequest = bundle.Bundle.LoadAssetAsync(assetName);
        yield return bundleRequest;
        Debug.Log("this is LoadBundleAsync");
        action?.Invoke(bundleRequest?.asset);
    }

    BundleData GetBundle(string name)
    {
        BundleData bundle = null;
        if (m_AssetBundles.TryGetValue(name, out bundle))
        {
            bundle.Count++; //要加载这个bundle，则计数+1
            return bundle;
        }
        return null;
    }

    //减去一个bundle的引用计数
    private void MinusOneBundleCount(string bundleName)
    {
        if (m_AssetBundles.TryGetValue(bundleName, out BundleData bundle))
        {
            if (bundle.Count > 0)
            {
                bundle.Count--;
                Debug.Log("bundle引用计数 :" + bundleName + " count : " + bundle.Count);
            }
            if (bundle.Count <= 0)
            {
                Debug.Log("放入bundle对象池 :" + bundleName);
                Manager.Pool.UnSpawn("AssetBundle", bundleName, bundle.Bundle);
                m_AssetBundles.Remove(bundleName);
            }
        }
    }

    //减去bundle和依赖的引用计数
    public void MinusBundleCount(string assetName)
    {
        string bundleName = m_BundleInfos[assetName].BundleName;

        MinusOneBundleCount(bundleName);

        //依赖资源
        List<string> dependences = m_BundleInfos[assetName].Dependences;
        if (dependences != null)
        {
            foreach (string dependence in dependences)
            {
                string name = m_BundleInfos[dependence].BundleName;
                MinusOneBundleCount(name);
            }
        }
    }


    //加载资源的接口
#if UNITY_EDITOR
    /// <summary>
    /// 编辑器环境加载资源
    /// </summary>
    /// <param name="assetName"></param>
    /// <param name="action"></param>
    void EditorLoadAsset(string assetName, Action<UObject> action = null)
    {
        Debug.Log("this is EditorLoadAsset");
        UObject obj = UnityEditor.AssetDatabase.LoadAssetAtPath(assetName, typeof(UObject));
        if (obj == null)
            Debug.LogError("assets name is not exist:" + assetName);
        action?.Invoke(obj);
    }
#endif

    private void LoadAsset(string assetName, Action<UObject> action)
    {
#if UNITY_EDITOR
        if (AppConst.GameMode == GameMode.EditorMode)
            EditorLoadAsset(assetName, action);
        else
#endif
            StartCoroutine(LoadBundleAsync(assetName, action));
    }

    //加载UI
    public void LoadUI(string assetName, Action<UnityEngine.Object> action = null)
    {
        LoadAsset(PathUtil.GetUIPath(assetName), action);
    }

    public void LoadMusic(string assetName, Action<UnityEngine.Object> action = null)
    {
        LoadAsset(PathUtil.GetMusicPath(assetName), action);
    }

    public void LoadSound(string assetName, Action<UnityEngine.Object> action = null)
    {
        LoadAsset(PathUtil.GetSoundPath(assetName), action);
    }

    public void LoadEffect(string assetName, Action<UnityEngine.Object> action = null)
    {
        LoadAsset(PathUtil.GetEffectPath(assetName), action);
    }

    public void LoadScene(string assetName, Action<UnityEngine.Object> action = null)
    {
        LoadAsset(PathUtil.GetScenePath(assetName), action);
    }

    public void LoadLua(string assetName, Action<UnityEngine.Object> action = null)
    {
        LoadAsset(assetName, action);
    }

    public void LoadPrefab(string assetName, Action<UnityEngine.Object> action = null)
    {
        LoadAsset(assetName, action);
    }



    
    public void UnloadBundle(UObject obj)
    {
        AssetBundle ab = obj as AssetBundle;
        ab.Unload(true);
    }

}
