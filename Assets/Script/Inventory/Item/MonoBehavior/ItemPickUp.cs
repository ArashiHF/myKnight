using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public ItemData_SO itemData;//��Ʒ����
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //��������ӽ�����

            //װ������
            InventoryManager.Instance.inventoryData.AddItem(itemData, itemData.itemAmount);//������Ʒ������
            InventoryManager.Instance.inventoryUI.RefreshUI();//ˢ������
            //GameManager.Instance.playerStats.EquipWeapon(itemData);//��������
            //�������
            QuestManager.Instance.UpdateQuestProgress(itemData.itemName, itemData.itemAmount);
            //ɾ����Ʒ
            Destroy(gameObject);
        }
    }
}
