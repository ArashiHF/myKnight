using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Data",menuName = "Character Stats/Data")]
public class CharacterData_SO : ScriptableObject
{
    [Header("stats Info")]

    public int maxHealth;//���Ѫ��

    public int currentHealth;//��ǰѪ��

    public int baseDefence;//��������

    public int currentDefence;//��ǰ����

    [Header("Kill")]
    public int KillPoint;//��ɱ��õľ���

    [Header("Level")]
    public int currentLevel;//��ǰ�ȼ�

    public int maxLevel;//��ߵȼ�

    public int currentExp;//��ǰ����

    public int baseExp;//��ǰ�����ֵ

    public float LevelBuff;//�����ӳ�

    public float LevelMultiplier
    {
        get { return 1 + (currentLevel - 1) * LevelBuff; }//��ȡ��������
    }
    

    public void UpdateExp(int point)
    {
        currentExp += point;

        if (currentExp >= baseExp)
            LevelUp();//提升等级
    }

    private void LevelUp()
    {
        //等级提升代码
        currentLevel = Mathf.Clamp(currentLevel + 1, 0, maxLevel);//提升等级

        baseExp += (int)(baseExp * LevelMultiplier);//增加当前经验

        maxHealth = (int)(maxHealth * LevelMultiplier);//提升最大血量

        currentHealth = maxHealth;//回满血

    }
}
