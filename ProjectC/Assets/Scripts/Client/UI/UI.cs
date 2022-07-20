using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

// 모든 UI 오브젝트의 부모
public abstract class UI : MonoBehaviour
{
    // 각 UI를 담을 딕셔너리
    protected Dictionary<Type, UnityEngine.Object[]> objectBinder = new Dictionary<Type, UnityEngine.Object[]>();

    public abstract void Init();

    // 오브젝트 바인딩
    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type);
        UnityEngine.Object[] objects = new UnityEngine.Object[name.Length];
        objectBinder.Add(typeof(T), objects);

        for(int i = 0;i < names.Length; i++)
        {
            // 타입이 GameObject인 경우
            if(typeof(T) == typeof(GameObject))
            {
                objects[i] = Utility.FindChild(gameObject,names[i],true);
            }
            // 그 외 타입인 경우
            else
            {
                objects[i] = Utility.FindChild<T>(gameObject,names[i],true);
            }
            // 배열 내에 값이 존재하지 않는 경우
            if (objects[i] == null) Debug.Log($"바인딩에 실패했습니다({names[i]})");
        }
    }

    // 이벤트 바인딩
    public static void BindEvent(GameObject target, Action<PointerEventData> action, EventDefine.UIEvent type = EventDefine.UIEvent.Click)
    {
        EventHandler handler = Utility.GetOrAddComponent<EventHandler>(target);

        switch (type)
        {
            case EventDefine.UIEvent.Click:
                handler.onClickSwicth -= action;
                handler.onClickSwicth += action;
                break;
            case EventDefine.UIEvent.Drag:
                handler.onDragSwicth -= action;
                handler.onDragSwicth += action;
                break;
        }
        Debug.Log($"이벤트가 바인딩 되었습니다{handler}");
    }

    // UI 가져오기
    protected T GetUI<T>(int index) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;
        if (!objectBinder.TryGetValue(typeof(T), out objects)) return null;
        return objects[index] as T;
    }

    // GameObject 타입 가져오기
    protected GameObject GetObject(int index)
    {
        return GetUI<GameObject>(index);
    }

    // TMP Text 타입 가져오기
    protected TextMeshProUGUI GetText(int index)
    {
        return GetUI<TextMeshProUGUI>(index);
    }

    // Button 타입 가져오기
    protected Button GetButton(int index)
    {
        return GetUI<Button>(index);
    }
    
    // Image 타입 가져오기
    protected Image GetImage(int index)
    {
        return GetUI<Image>(index);
    }

    protected TMP_InputField GetInputField(int index)
    {
        return GetUI<TMP_InputField>(index);
    }
}