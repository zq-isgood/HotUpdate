using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathUtil 
{
    //根目录
    public static readonly string AseetsPath = Application.dataPath;

    //需要打bundle的目录
    public static readonly string BuildResourcesPath = AseetsPath + "/BuildResources/";

    //bundle 输出目录
    public static readonly string BundleOutPath = Application.streamingAssetsPath;

    //只读目录
    public static readonly string ReadPath = Application.streamingAssetsPath;

    //可读写目录---应该是公司名，项目名的在用户的LocalLow的那个目录下
    public static readonly string ReadWritePath = Application.persistentDataPath;

    //lua路径
    public static readonly string LuaPath = "Assets/BuildResources/LuaScripts";

    //bundle资源路径----热更新的读取版本文件的路径---整包下载则要有释放资源的过程，从只读复制到读写
    public static string BundleResourcePath

    {
        get
        {
            if (AppConst.GameMode == GameMode.UpdateMode)
                return ReadWritePath;
            return ReadPath;
        }
    }

    /// <summary>
    /// 获取Unity的相对路径
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string GetUnityPath(string path)
    {
        if (string.IsNullOrEmpty(path))
            return string.Empty;
        return path.Substring(path.IndexOf("Assets"));
    }

    /// <summary>
    /// 获取标准路径
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string GetStandardPath(string path)
    {
        if (string.IsNullOrEmpty(path))
            return string.Empty;
        return path.Trim().Replace("\\", "/");
    }

    public static string GetLuaPath(string name)
    {
        return string.Format("Assets/BuildResources/LuaScripts/{0}.bytes", name);
    }

    public static string GetUIPath(string name)
    {
        return string.Format("Assets/BuildResources/UI/Prefabs/{0}.prefab", name);
    }

    public static string GetMusicPath(string name)
    {
        return string.Format("Assets/BuildResources/Audio/Music/{0}", name);
    }

    public static string GetSoundPath(string name)
    {
        return string.Format("Assets/BuildResources/Audio/Sound/{0}", name);
    }

    public static string GetEffectPath(string name)
    {
        return string.Format("Assets/BuildResources/Effect/Prefabs/{0}.prefab", name);
    }

    public static string GetModelPath(string name)
    {
        return string.Format("Assets/BuildResources/Model/Prefabs/{0}.prefab", name);
    }

    public static string GetSpritePath(string name)
    {
        return string.Format("Assets/BuildResources/Sprites/{0}", name);
    }

    public static string GetScenePath(string name)
    {
        return string.Format("Assets/BuildResources/Scenes/{0}.unity", name);
    }


}
