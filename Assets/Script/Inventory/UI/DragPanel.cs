using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragPanel : MonoBehaviour, IDragHandler,IPointerDownHandler
{
    RectTransform rectTransform;//ҳ��λ�ã������϶�����ҳ��

    Canvas canvas;//ҳ�棬���ڳ�����Ļ������������ƶ���Ӧ��Ļ

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();//��ȡҳ��λ��
        canvas = InventoryManager.Instance.GetComponent<Canvas>();//��ȡ��Ļ
    }
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta/canvas.scaleFactor;//��ҳ��λ�ü����������,��ҳ���������ƶ�
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        rectTransform.SetSiblingIndex(2);//��Ŀǰ��ק����Ʒλ����Ϊ�ڶ���,��֤���ǵĽ����ܹ�����һ��
    }
}
