using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[XLua.LuaCallCSharp]
public static class UnityEx
{
    //这个脚本解决的问题是：c#关闭了虚拟机，但是lua的事件仍然在监听的报错---LuaManager
    //下面两个方法在TestUI的按钮监听AddListener部分替换，直接换成OnClickSet,OnValueChangedSet
    public static void OnClickSet(this Button button, object callback)
    {
        XLua.LuaFunction func = callback as XLua.LuaFunction;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(
            () =>
            {
                func?.Call();
            });
    }

    public static void OnValueChangedSet(this Slider slider, object callback)
    {
        XLua.LuaFunction func = callback as XLua.LuaFunction;
        slider.onValueChanged.RemoveAllListeners();
        slider.onValueChanged.AddListener(
            (float value) =>
            {
                func?.Call(value);
            });
    }
}
