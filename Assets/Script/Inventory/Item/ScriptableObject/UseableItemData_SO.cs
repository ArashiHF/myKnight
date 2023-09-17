using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Useable Item",menuName = "Inventory/Useable Item Data")]
public class UseableItemData_SO : ScriptableObject
{
    public int healthPoint;//回复血量

    public int speedUp;//提升速度

    public int damageUp;//提升伤害
}
