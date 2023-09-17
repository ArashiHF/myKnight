using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public Image icon = null;//ͼƬ

    public Text amount = null;//����

    public ItemData_SO currentItemData;//������

    public InventoryData_SO Bag { get; set; }//��ȡ�͸��±�������
    public int index { get; set; } = -1;//����

    public void SetupItemUI(ItemData_SO item,int itemAmount)
    {

        if(itemAmount == 0)//�������Ϊ0
        {
            Bag.items[index].itemData = null;//��������
            icon.gameObject.SetActive(false);//�رո���
            return;
        }

        if(itemAmount < 0)//����ǿ۳��Ļ�����ʾ
        {
            item = null;
        }

        if (item != null)//��ȡͼƬ������
        {
            currentItemData = item;//��ֵ��ǰ����
            icon.sprite = item.itemIcon;
            amount.text = itemAmount.ToString();
            icon.gameObject.SetActive(true);
        }
        else
            icon.gameObject.SetActive(false);
    }

    public ItemData_SO GetItem()//��������
    {
        return Bag.items[index].itemData;
    }
}
