using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Attack",menuName ="Attack/Attack Data")]
public class AttackData_SO : ScriptableObject
{
    public float attackRange;//攻击距离

    public float skillRange;//技能距离

    public float coolDown;//冷却时间

    public int minDamage;//最小攻击值

    public int maxDamage;//最大攻击值

    public float criticalMultiplier;//暴击加成

    public float criticalChance;//暴击率

    public void ApplyWeaponData(AttackData_SO weapon)//应用新的武器数据
    {
        //替换所有属性
        attackRange = weapon.attackRange;
        skillRange = weapon.skillRange;
        coolDown = weapon.coolDown;

        minDamage = weapon.minDamage;
        maxDamage = weapon.maxDamage;

        criticalMultiplier = weapon.criticalMultiplier;
        criticalChance = weapon.criticalChance;
    }
}
