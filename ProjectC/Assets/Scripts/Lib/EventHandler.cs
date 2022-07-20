using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EventHandler : MonoBehaviour, IPointerClickHandler, IDragHandler
{
    public Action<PointerEventData> onClickSwicth = null;
    public Action<PointerEventData> onDragSwicth = null;

    // 클릭 이벤트
    public void OnPointerClick(PointerEventData eventData)
    {
        if (onClickSwicth != null) onClickSwicth.Invoke(eventData);
    }

    // 드래그 이벤트
    public void OnDrag(PointerEventData eventData)
    {
        if (onDragSwicth != null) onDragSwicth.Invoke(eventData);
    }

}
