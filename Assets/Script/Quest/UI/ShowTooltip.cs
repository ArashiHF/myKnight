using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private ItemUI currentItemUI;

    void Awake()
    {
        currentItemUI = GetComponent<ItemUI>();//获取组件
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        QuestUI.Instance.tooltip.gameObject.SetActive(true);//启动简介页面
        QuestUI.Instance.tooltip.SetUpTooltip(currentItemUI.currentItemData);//获取简介数据
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        QuestUI.Instance.tooltip.gameObject.SetActive(false);//关闭简介页面
    }
}
