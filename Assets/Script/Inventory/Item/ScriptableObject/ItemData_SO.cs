using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType { Useable,Weapon,Armor}//��Ʒ���ͣ���ʹ�� ���� ����
[CreateAssetMenu(fileName = "New Item",menuName = "Inventory/Item Data")]
public class ItemData_SO : ScriptableObject
{
    public ItemType itemType;//��Ʒ����

    public string itemName;//��Ʒ����

    public Sprite itemIcon;//��Ʒͼ��

    public int itemAmount;//����

    [TextArea]
    public string description = "";//��Ʒ���

    public bool stackable;//�Ƿ�ɶѵ�

    [Header("Useable Item")]
    public UseableItemData_SO useableData;//�ظ�Ʒ����

    [Header("Weapon")]
    public GameObject weaponPrefab;//��ƷԤ����

    public AttackData_SO weaponData;//��������

    public AnimatorOverrideController weaponAnimator;//�������׶���
}
