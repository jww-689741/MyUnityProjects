using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    public Color32 successLogColor = new Color32(0, 177, 255, 255);
    public Color32 errorLogColor = new Color32(255, 0, 0, 255);

    private Stack<UI_Popup> popupStack = new Stack<UI_Popup>(); // 팝업 UI 스택
    private UI_Scene currentSceneUI = null; // 현재 씬의 고정UI

    private int order = 10; // UI 배치 정렬 서순

    // UI의 루트를 탐색, 없으면 생성
    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("UI_Root");
            if(root == null)
            {
                root = new GameObject { name = "UI_Root" };
            }
            return root;
        }
    }

    // 캔버스 셋팅
    public void SetCanvas(GameObject target, bool sortFlag = true)
    {
        Canvas canvas = Utility.GetOrAddComponent<Canvas>(target);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        if (sortFlag)
        {
            canvas.sortingOrder = order;
            order++;
        }
        else canvas.sortingOrder = 0;
    }
    
    // 고정 UI 출력
    public T ShowSceneUI<T>(string name = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject target = ResourceManager.Instance.Instantiate($"UI/Scene/{name}");
        T sceneUI = Utility.GetOrAddComponent<T>(target);
        currentSceneUI = sceneUI;

        target.transform.SetParent(Root.transform);

        return sceneUI;
    }

    // 팝업 UI 출력 및 스택 삽입
    public T ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(name)) name = typeof(T).Name;
        GameObject createUI = ResourceManager.Instance.Instantiate($"UI/Popup/{name}");
        T popup = Utility.GetOrAddComponent<T>(createUI);
        popupStack.Push(popup);
        createUI.transform.SetParent(Root.transform);
        return popup;
    }

    // Peek의 값이 target과 다를경우 메세지 출력
    public void ClosePopup(UI_Popup target)
    {
        if (popupStack.Count == 0) return;
        if(popupStack.Peek() != target)
        {
            Debug.Log("팝업 닫기에 실패했습니다 : target이(가) 스택의 최상위 요소가 아닙니다.");
            return;
        }
        ClosePopup();
    }

    // 팝업창 닫기
    public void ClosePopup()
    {
        if (popupStack.Count == 0) return;
        UI_Popup popup = popupStack.Pop();
        ResourceManager.Instance.Destroy(popup.gameObject);
        popup = null;
        order--;
    }

    // 모든 팝업창 닫기
    public void ClosePopupAll()
    {
        while(popupStack.Count > 0)
        {
            ClosePopup();
        }
    }

}
