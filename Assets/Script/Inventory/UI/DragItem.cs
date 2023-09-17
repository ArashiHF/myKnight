using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ItemUI))]
public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    ItemUI currentItemUI;//当前数据

    SlotHolder currentHolder;//当前格子

    SlotHolder targetHolder;//目标格子

    void Awake()
    {
        //赋予当前格子的所有数据
        currentItemUI = GetComponent<ItemUI>();
        currentHolder = GetComponentInParent<SlotHolder>();
    }
    public void OnBeginDrag(PointerEventData eventData)//开始拖拽
    {

        //让InventoryManager保存自己的数据
        InventoryManager.Instance.currentDrag = new InventoryManager.DragData();
        InventoryManager.Instance.currentDrag.originalHolder = GetComponent<SlotHolder>();
        InventoryManager.Instance.currentDrag.originalParent = (RectTransform)transform.parent;

        //记录原始数据
        transform.SetParent(InventoryManager.Instance.dragCanvas.transform, true);//讲这个数据移动到Drag Canvas里面当子类，用于展示
    }

    public void OnDrag(PointerEventData eventData)//拖拽过程
    {
        //跟随鼠标移动
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)//拖拽结束
    {
        //放下物品交换数据
        //是否指向UI物品
        if(EventSystem.current.IsPointerOverGameObject())
        {
            if(InventoryManager.Instance.CheckInActionUI(eventData.position)||InventoryManager.Instance.CheckInEquipmentUI(eventData.position)||
                InventoryManager.Instance.CheckInInventoryUI(eventData.position))//如果东西处于格子内的话就进行判断
            {
                //获取目标格子属性
                if (eventData.pointerEnter.gameObject.GetComponent<SlotHolder>())
                    targetHolder = eventData.pointerEnter.gameObject.GetComponent<SlotHolder>();//如果拖拽的东西到了格子到了格子上则给目标格子赋值
                else
                    targetHolder = eventData.pointerEnter.gameObject.GetComponentInParent<SlotHolder>();//否则将目标作为其父类的格子->无论怎么样都能拿到格子的数据

                if(targetHolder != InventoryManager.Instance.currentDrag.originalHolder)//判断目标是否为穿过了我的目标
                switch(targetHolder.slotType)//确定格子的类型
                {
                    case SlotType.BAG://背包
                        SwapItem();//交换
                        break;
                    case SlotType.WEAPON://武器
                        if (currentItemUI.Bag.items[currentItemUI.index].itemData.itemType == ItemType.Weapon)//只有是武器
                            SwapItem();//交换
                        break;
                    case SlotType.ARMOR://护盾
                        if (currentItemUI.Bag.items[currentItemUI.index].itemData.itemType == ItemType.Armor)//只有是护盾才可
                            SwapItem();//交换
                        break;
                    case SlotType.ACTION://如果目标为快捷栏
                        if(currentItemUI.Bag.items[currentItemUI.index].itemData.itemType == ItemType.Useable)//如果物品是可使用的话在能放进快捷栏中
                        SwapItem();//交换
                        break;
                }

                //更新格子
                currentHolder.UpdateItem();
                targetHolder.UpdateItem();
                
            }
        }

        transform.SetParent(InventoryManager.Instance.currentDrag.originalParent);//将原来的格子放回原来的位置

        RectTransform t = transform as RectTransform;//确定格子位置是否回到了标准位置

        t.offsetMax = -Vector2.one * 5;
        t.offsetMin = Vector2.one * 5;
    }
    public void SwapItem()//如果是背包里面的东西就
    {
        var targetItem = targetHolder.itemUI.Bag.items[targetHolder.itemUI.index];//获取目标的数据

        var tempItem = currentHolder.itemUI.Bag.items[currentHolder.itemUI.index];//获取在点击之前的数据

        bool isSameItem = tempItem.itemData == targetItem.itemData;//如果两个物品是同种类型

        if(isSameItem&&targetItem.itemData.stackable)
        {
            targetItem.amount += tempItem.amount;//如果两个物品是相通而且可堆叠，则将数据相加
            tempItem.itemData = null;
            tempItem.amount = 0;
        }
        else
        {
            currentHolder.itemUI.Bag.items[currentHolder.itemUI.index] = targetItem;//如果物品是不同的或者是不可堆叠的则将两个物品相互交换
            targetHolder.itemUI.Bag.items[targetHolder.itemUI.index] = tempItem;
        }
    }
}
