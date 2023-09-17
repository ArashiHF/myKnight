using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Attack",menuName ="Attack/Attack Data")]
public class AttackData_SO : ScriptableObject
{
    public float attackRange;//��������

    public float skillRange;//���ܾ���

    public float coolDown;//��ȴʱ��

    public int minDamage;//��С����ֵ

    public int maxDamage;//��󹥻�ֵ

    public float criticalMultiplier;//�����ӳ�

    public float criticalChance;//������

    public void ApplyWeaponData(AttackData_SO weapon)//Ӧ���µ���������
    {
        //�滻��������
        attackRange = weapon.attackRange;
        skillRange = weapon.skillRange;
        coolDown = weapon.coolDown;

        minDamage = weapon.minDamage;
        maxDamage = weapon.maxDamage;

        criticalMultiplier = weapon.criticalMultiplier;
        criticalChance = weapon.criticalChance;
    }
}