using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ItemUI))]
public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    ItemUI currentItemUI;//��ǰ����

    SlotHolder currentHolder;//��ǰ����

    SlotHolder targetHolder;//Ŀ�����

    void Awake()
    {
        //���赱ǰ���ӵ���������
        currentItemUI = GetComponent<ItemUI>();
        currentHolder = GetComponentInParent<SlotHolder>();
    }
    public void OnBeginDrag(PointerEventData eventData)//��ʼ��ק
    {

        //��InventoryManager�����Լ�������
        InventoryManager.Instance.currentDrag = new InventoryManager.DragData();
        InventoryManager.Instance.currentDrag.originalHolder = GetComponent<SlotHolder>();
        InventoryManager.Instance.currentDrag.originalParent = (RectTransform)transform.parent;

        //��¼ԭʼ����
        transform.SetParent(InventoryManager.Instance.dragCanvas.transform, true);//����������ƶ���Drag Canvas���浱���࣬����չʾ
    }

    public void OnDrag(PointerEventData eventData)//��ק����
    {
        //��������ƶ�
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)//��ק����
    {
        //������Ʒ��������
        //�Ƿ�ָ��UI��Ʒ
        if(EventSystem.current.IsPointerOverGameObject())
        {
            if(InventoryManager.Instance.CheckInActionUI(eventData.position)||InventoryManager.Instance.CheckInEquipmentUI(eventData.position)||
                InventoryManager.Instance.CheckInInventoryUI(eventData.position))//����������ڸ����ڵĻ��ͽ����ж�
            {
                //��ȡĿ���������
                if (eventData.pointerEnter.gameObject.GetComponent<SlotHolder>())
                    targetHolder = eventData.pointerEnter.gameObject.GetComponent<SlotHolder>();//�����ק�Ķ������˸��ӵ��˸��������Ŀ����Ӹ�ֵ
                else
                    targetHolder = eventData.pointerEnter.gameObject.GetComponentInParent<SlotHolder>();//����Ŀ����Ϊ�丸��ĸ���->������ô�������õ����ӵ�����

                if(targetHolder != InventoryManager.Instance.currentDrag.originalHolder)//�ж�Ŀ���Ƿ�Ϊ�������ҵ�Ŀ��
                switch(targetHolder.slotType)//ȷ�����ӵ�����
                {
                    case SlotType.BAG://����
                        SwapItem();//����
                        break;
                    case SlotType.WEAPON://����
                        if (currentItemUI.Bag.items[currentItemUI.index].itemData.itemType == ItemType.Weapon)//ֻ��������
                            SwapItem();//����
                        break;
                    case SlotType.ARMOR://����
                        if (currentItemUI.Bag.items[currentItemUI.index].itemData.itemType == ItemType.Armor)//ֻ���ǻ��ܲſ�
                            SwapItem();//����
                        break;
                    case SlotType.ACTION://���Ŀ��Ϊ�����
                        if(currentItemUI.Bag.items[currentItemUI.index].itemData.itemType == ItemType.Useable)//�����Ʒ�ǿ�ʹ�õĻ����ܷŽ��������
                        SwapItem();//����
                        break;
                }

                //���¸���
                currentHolder.UpdateItem();
                targetHolder.UpdateItem();
                
            }
        }

        transform.SetParent(InventoryManager.Instance.currentDrag.originalParent);//��ԭ���ĸ��ӷŻ�ԭ����λ��

        RectTransform t = transform as RectTransform;//ȷ������λ���Ƿ�ص��˱�׼λ��

        t.offsetMax = -Vector2.one * 5;
        t.offsetMin = Vector2.one * 5;
    }
    public void SwapItem()//����Ǳ�������Ķ�����
    {
        var targetItem = targetHolder.itemUI.Bag.items[targetHolder.itemUI.index];//��ȡĿ�������

        var tempItem = currentHolder.itemUI.Bag.items[currentHolder.itemUI.index];//��ȡ�ڵ��֮ǰ������

        bool isSameItem = tempItem.itemData == targetItem.itemData;//���������Ʒ��ͬ������

        if(isSameItem&&targetItem.itemData.stackable)
        {
            targetItem.amount += tempItem.amount;//���������Ʒ����ͨ���ҿɶѵ������������
            tempItem.itemData = null;
            tempItem.amount = 0;
        }
        else
        {
            currentHolder.itemUI.Bag.items[currentHolder.itemUI.index] = targetItem;//�����Ʒ�ǲ�ͬ�Ļ����ǲ��ɶѵ�����������Ʒ�໥����
            targetHolder.itemUI.Bag.items[targetHolder.itemUI.index] = tempItem;
        }
    }
}
