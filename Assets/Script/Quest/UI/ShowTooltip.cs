using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private ItemUI currentItemUI;

    void Awake()
    {
        currentItemUI = GetComponent<ItemUI>();//��ȡ���
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        QuestUI.Instance.tooltip.gameObject.SetActive(true);//�������ҳ��
        QuestUI.Instance.tooltip.SetUpTooltip(currentItemUI.currentItemData);//��ȡ�������
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        QuestUI.Instance.tooltip.gameObject.SetActive(false);//�رռ��ҳ��
    }
}
