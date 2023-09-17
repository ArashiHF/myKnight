using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType { Useable,Weapon,Armor}//物品类型，可使用 武器 护甲
[CreateAssetMenu(fileName = "New Item",menuName = "Inventory/Item Data")]
public class ItemData_SO : ScriptableObject
{
    public ItemType itemType;//物品类型

    public string itemName;//物品名字

    public Sprite itemIcon;//物品图标

    public int itemAmount;//数量

    [TextArea]
    public string description = "";//物品简介

    public bool stackable;//是否可堆叠

    [Header("Useable Item")]
    public UseableItemData_SO useableData;//回复品数据

    [Header("Weapon")]
    public GameObject weaponPrefab;//物品预制体

    public AttackData_SO weaponData;//武器数据

    public AnimatorOverrideController weaponAnimator;//武器配套动画
}
