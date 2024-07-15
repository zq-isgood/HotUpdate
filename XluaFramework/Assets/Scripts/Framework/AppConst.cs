using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public enum GameMode
{
    EditorMode,  //编辑器模式
    PackageBundle,  //读取ab包模式
    UpdateMode  //更新模式
}

public enum GameEvent
{
    GameInit = 10000,
    StartLua,
}

public class AppConst
{
    public const string BundleExtension = ".ab";
    public const string FileListName = "filelist.txt";
    public static GameMode GameMode = GameMode.EditorMode;
    public static bool OpenLog = true;
    //热更资源的地址
    public const string ResourcesUrl = "http://127.0.0.1/AssetBundles";//192.168.1.3
}
