using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum SlotType { BAG,WEAPON,ARMOR,ACTION}
public class SlotHolder : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public SlotType slotType;//��������

    public ItemUI itemUI;

    public void OnPointerClick(PointerEventData eventData)//˫��ʵ��������Ʒ
    {
       if(eventData.clickCount%2 == 0)
        {
            UseItem();
        }
    }

    public void UseItem()//ʹ����Ʒ
    {
        if(itemUI.GetItem() != null)//���Ӳ�Ϊ��
        if(itemUI.GetItem().itemType == ItemType.Useable&& itemUI.Bag.items[itemUI.index].amount >0)//ֻ���ܱ�ʹ�õĶ�������ʹ��
        {
            GameManager.Instance.playerStats.ApplyHealth(itemUI.GetItem().useableData.healthPoint);//ʹ����Ʒ

            itemUI.Bag.items[itemUI.index].amount -= 1;//��������

                //����ʹ����Ʒ
                QuestManager.Instance.UpdateQuestProgress(itemUI.GetItem().itemName, -1);//ʹ����ƷʱҪ������1
        }
        UpdateItem();
    }

    public void UpdateItem()
    {
        switch(slotType)//���ݲ�ͬ�����������ͷ���
        {
            case SlotType.BAG:
                itemUI.Bag = InventoryManager.Instance.inventoryData;//��ȡ��ǰ�ı�������
                break;
            case SlotType.WEAPON:
                itemUI.Bag = InventoryManager.Instance.equipmentData;//����װ��
                //�л�����
                if(itemUI.Bag.items[itemUI.index].itemData != null)//������Ϊ��
                {
                    GameManager.Instance.playerStats.ChangeWeapon(itemUI.Bag.items[itemUI.index].itemData);//�����л�����
                }
                else//û��������ж������
                {
                    GameManager.Instance.playerStats.UnEquipWeapon();//ж������
                }
                break;
            case SlotType.ARMOR:
                itemUI.Bag = InventoryManager.Instance.equipmentData;//����װ��
                break;
            case SlotType.ACTION:
                itemUI.Bag = InventoryManager.Instance.actionData;//���Ŀ��������
                break;
        }
        var item = itemUI.Bag.items[itemUI.index];//�������

        itemUI.SetupItemUI(item.itemData, item.amount);//��ӵ�������
    }

    public void OnPointerEnter(PointerEventData eventData)//�����ͣ����
    {
        if(itemUI.GetItem())
        {
            InventoryManager.Instance.tooltip.SetUpTooltip(itemUI.GetItem());//���¼���������
            InventoryManager.Instance.tooltip.gameObject.SetActive(true);//�������
        }
    }

    public void OnPointerExit(PointerEventData eventData)//�����ͣ�뿪
    {
        InventoryManager.Instance.tooltip.gameObject.SetActive(false);//�رռ��
    }

    void OnDisable()
    {
        InventoryManager.Instance.tooltip.gameObject.SetActive(false);//�رձ���ʱҲ�رռ��
    }


}
