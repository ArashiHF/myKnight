using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum SlotType { BAG,WEAPON,ARMOR,ACTION}
public class SlotHolder : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public SlotType slotType;//格子类型

    public ItemUI itemUI;

    public void OnPointerClick(PointerEventData eventData)//双击实现消耗物品
    {
       if(eventData.clickCount%2 == 0)
        {
            UseItem();
        }
    }

    public void UseItem()//使用物品
    {
        if(itemUI.GetItem() != null)//格子不为空
        if(itemUI.GetItem().itemType == ItemType.Useable&& itemUI.Bag.items[itemUI.index].amount >0)//只有能被使用的东西才能使用
        {
            GameManager.Instance.playerStats.ApplyHealth(itemUI.GetItem().useableData.healthPoint);//使用物品

            itemUI.Bag.items[itemUI.index].amount -= 1;//数量减少

                //更新使用物品
                QuestManager.Instance.UpdateQuestProgress(itemUI.GetItem().itemName, -1);//使用物品时要数量减1
        }
        UpdateItem();
    }

    public void UpdateItem()
    {
        switch(slotType)//根据不同背包数据类型分类
        {
            case SlotType.BAG:
                itemUI.Bag = InventoryManager.Instance.inventoryData;//获取当前的背包数据
                break;
            case SlotType.WEAPON:
                itemUI.Bag = InventoryManager.Instance.equipmentData;//更新装备
                //切换武器
                if(itemUI.Bag.items[itemUI.index].itemData != null)//武器不为空
                {
                    GameManager.Instance.playerStats.ChangeWeapon(itemUI.Bag.items[itemUI.index].itemData);//赋予切换武器
                }
                else//没有武器就卸下武器
                {
                    GameManager.Instance.playerStats.UnEquipWeapon();//卸下武器
                }
                break;
            case SlotType.ARMOR:
                itemUI.Bag = InventoryManager.Instance.equipmentData;//更新装备
                break;
            case SlotType.ACTION:
                itemUI.Bag = InventoryManager.Instance.actionData;//更改快捷栏数据
                break;
        }
        var item = itemUI.Bag.items[itemUI.index];//给予序号

        itemUI.SetupItemUI(item.itemData, item.amount);//添加到背包中
    }

    public void OnPointerEnter(PointerEventData eventData)//鼠标悬停进入
    {
        if(itemUI.GetItem())
        {
            InventoryManager.Instance.tooltip.SetUpTooltip(itemUI.GetItem());//更新简介格子数据
            InventoryManager.Instance.tooltip.gameObject.SetActive(true);//启动简介
        }
    }

    public void OnPointerExit(PointerEventData eventData)//鼠标悬停离开
    {
        InventoryManager.Instance.tooltip.gameObject.SetActive(false);//关闭简介
    }

    void OnDisable()
    {
        InventoryManager.Instance.tooltip.gameObject.SetActive(false);//关闭背包时也关闭简介
    }


}
