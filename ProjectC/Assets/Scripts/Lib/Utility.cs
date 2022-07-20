using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 기능 유틸리티
public class Utility
{
    // GameObject 타입 대상 오브젝트 탐색
    public static GameObject FindChild(GameObject target, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(target, name, recursive);

        // transform이 null일 경우
        if (transform == null) return null;

        return transform.gameObject;
    }

    // GameObject를 제외한 모든 타입 대상 오브젝트 탐색
    public static T FindChild<T>(GameObject target, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        // target 오브벡트가 null일 경우
        if (target == null) return null;

        // 모든 자식 오브젝트를 전부 탐색할 경우
        if (!recursive)
        {
            for (int i = 0; i < target.transform.childCount; i++)
            {
                Transform transform = target.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name.Equals(name))
                {
                    T component = transform.GetComponent<T>();
                    if (component != null) return component;
                }
            }
        }
        // 직계 자식 오브젝트만 탐색할 경우
        else
        {
            foreach (T component in target.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name.Equals(name)) return component;
            }
        }

        return null;
    }

    // 컴포넌트를 가져오고 만약 없다면 추가해서 가져오는 기능
    public static T GetOrAddComponent<T>(GameObject target) where T : UnityEngine.Component
    {
        T component = target.GetComponent<T>();
        if (component == null) component = target.AddComponent<T>();
        return component;
    }
}
