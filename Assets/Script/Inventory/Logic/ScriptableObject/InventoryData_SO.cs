using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Inventory",menuName ="Inventory/Inventory Data")]
public class InventoryData_SO : ScriptableObject
{
    public List<InventoryItem> items = new List<InventoryItem>();//数据列表

    public void AddItem(ItemData_SO newItemData,int amount)//添加新的物品，并且判断是否存在
    {
        bool found = false;
        if(newItemData.stackable)
        {
            foreach(var item in items)
            {
                if(item.itemData == newItemData)//如果为已有的物品则添加数据
                {
                    item.amount += amount;
                    found = true;
                    break;
                }
            }
        }
        for(int i = 0;i<items.Count;i++)
        {
            if(items[i].itemData == null&&!found)
            {
                items[i].itemData = newItemData;
                items[i].amount = amount;
                break;
            }
        }
    }
}
[System.Serializable]
public class InventoryItem
{

    public ItemData_SO itemData;//物品数据

    public int amount;//物品数量
}
