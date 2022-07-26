using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// 씬 내에서 팝업의 형식으로 관리 할 UI
public class UI_Popup : UI
{
    public override void Init()
    {
        UIManager.Instance.SetCanvas(gameObject, true);
    }

    public virtual void ClosePopup(PointerEventData data)
    {
        UIManager.Instance.ClosePopup(this);
    }
    public virtual void ClosePopup()
    {
        UIManager.Instance.ClosePopup(this);
    }
}
