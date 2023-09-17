using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragPanel : MonoBehaviour, IDragHandler,IPointerDownHandler
{
    RectTransform rectTransform;//页面位置，用于拖动背包页面

    Canvas canvas;//页面，用于除以屏幕比例，让鼠标移动适应屏幕

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();//获取页面位置
        canvas = InventoryManager.Instance.GetComponent<Canvas>();//获取屏幕
    }
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta/canvas.scaleFactor;//给页面位置加上鼠标增量,让页面跟随鼠标移动
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        rectTransform.SetSiblingIndex(2);//将目前拖拽的物品位置作为第二个,保证我们的界面能够在上一层
    }
}
