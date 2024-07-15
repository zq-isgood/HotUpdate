using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    //缓存UI
    //Dictionary<string, GameObject> m_UI = new Dictionary<string, GameObject>();

    //ui分组，分的组在lua中定义
    Dictionary<string, Transform> m_UIGroups = new Dictionary<string, Transform>();

    private Transform m_UIParent;

    private void Awake()
    {
        m_UIParent = this.transform.parent.Find("UI"); //manager节点与UI节点同级
    }
    //lua调用
    public void SetUIGroup(List<string> group)
    {
        for (int i = 0; i < group.Count; i++)
        {
            GameObject go = new GameObject("Group-" + group[i]);
            go.transform.SetParent(m_UIParent,false);
            m_UIGroups.Add(group[i], go.transform);
        }
    }

    Transform GetUIGroup(string group)
    {
        if (!m_UIGroups.ContainsKey(group))
            Debug.LogError("group is not exist");
        return m_UIGroups[group];
    }

    //openui是在lua脚本中调用的，Manager.UI:OpenUI("TestUI","ui.TestUI");预制体名字和脚本名字
    public void OpenUI(string uiName,string group, string luaName)
    {
        GameObject ui = null;
        Transform parent = GetUIGroup(group);//将此ui设置在他所在的分组
        string uiPath = PathUtil.GetUIPath(uiName);
        Object uiObj = Manager.Pool.Spawn("UI", uiPath); //取出UI对象
        if (uiObj != null) //如果有的话，不用实例化和初始化
        {
            ui = uiObj as GameObject;
            ui.transform.SetParent(parent, false);

            UILogic uiLogic = ui.GetComponent<UILogic>();
            uiLogic.OnOpen();
            return;
        }

        Manager.Resource.LoadUI(uiName, (UnityEngine.Object obj) =>
        {
            ui = Instantiate(obj) as GameObject;
            ui.transform.SetParent(parent, false);
            UILogic uiLogic = ui.AddComponent<UILogic>();
            uiLogic.AssetName = uiPath;
            uiLogic.Init(luaName); //awake
            uiLogic.OnOpen(); //start
        });
    }

}
