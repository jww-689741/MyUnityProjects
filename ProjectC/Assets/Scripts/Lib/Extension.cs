using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Extension
{
    // GetComtonentExpansion 메소드의 확장
    public static T GetOrAddComponent<T>(this GameObject target) where T : UnityEngine.Component
    {
        return Utility.GetOrAddComponent<T>(target);
    }

    public static void BindEvent(this GameObject target, Action<PointerEventData> action, EventDefine.UIEvent type = EventDefine.UIEvent.Click)
    {
        UI.BindEvent(target, action, type);
    }
}
