using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public Image icon = null;//图片

    public Text amount = null;//数量

    public ItemData_SO currentItemData;//任务奖励

    public InventoryData_SO Bag { get; set; }//获取和更新背包数据
    public int index { get; set; } = -1;//索引

    public void SetupItemUI(ItemData_SO item,int itemAmount)
    {

        if(itemAmount == 0)//如果数量为0
        {
            Bag.items[index].itemData = null;//消除数据
            icon.gameObject.SetActive(false);//关闭格子
            return;
        }

        if(itemAmount < 0)//如果是扣除的话不显示
        {
            item = null;
        }

        if (item != null)//获取图片和数量
        {
            currentItemData = item;//赋值当前格子
            icon.sprite = item.itemIcon;
            amount.text = itemAmount.ToString();
            icon.gameObject.SetActive(true);
        }
        else
            icon.gameObject.SetActive(false);
    }

    public ItemData_SO GetItem()//返回坐标
    {
        return Bag.items[index].itemData;
    }
}
