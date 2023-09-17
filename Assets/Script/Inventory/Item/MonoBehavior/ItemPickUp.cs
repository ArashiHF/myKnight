using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public ItemData_SO itemData;//物品数据
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //将物体添加进背包

            //装备武器
            InventoryManager.Instance.inventoryData.AddItem(itemData, itemData.itemAmount);//存入物品和数量
            InventoryManager.Instance.inventoryUI.RefreshUI();//刷新数据
            //GameManager.Instance.playerStats.EquipWeapon(itemData);//生成武器
            //检查任务
            QuestManager.Instance.UpdateQuestProgress(itemData.itemName, itemData.itemAmount);
            //删除物品
            Destroy(gameObject);
        }
    }
}
