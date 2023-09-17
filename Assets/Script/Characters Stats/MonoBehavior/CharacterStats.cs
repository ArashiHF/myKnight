using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public event Action<int, int> UpdataHealthBarOnAttack;//每次攻击之后更新血条

    public CharacterData_SO tempLateData;//复制的模板数据

    public CharacterData_SO characterData;//获取数据变量

    public AttackData_SO attackData;//攻击数据

    private AttackData_SO baseAttackData;//基础攻击

    private RuntimeAnimatorController baseAnimator;//初始动作

    [HideInInspector]
    public bool isCritical;//暴击判定

    [Header("Weapon")]
    public Transform weaponSlot;//武器的位置

    public Transform shieldSlot;//护盾位置


    void Awake()
    {
        if (tempLateData != null)
            characterData =Instantiate(tempLateData);//赋予模板数据

        baseAttackData = Instantiate(attackData);//赋予初始攻击属性

        baseAnimator = GetComponent<Animator>().runtimeAnimatorController;//获取初始动作
    }
    #region Read from Data_SO
    public int MaxHealth 
    {
        get { if (characterData != null) return characterData.maxHealth; else return 0;}
        set {characterData.maxHealth = value; }//更新
    }
    public int CurrentHealth
    {
        get { if (characterData != null) return characterData.currentHealth; else return 0; }
        set { characterData.currentHealth = value; }//更新
    }

    public int BaseDefence
    {
        get { if (characterData != null) return characterData.baseDefence; else return 0; }
        set { characterData.baseDefence = value; }//更新
    }

    public int CurrentDefence
    {
        get { if (characterData != null) return characterData.currentDefence; else return 0; }
        set { characterData.currentDefence = value; }//更新
    }
    #endregion

    #region Character Combat
    public void TakeDamage(CharacterStats attacker,CharacterStats defener)//攻击脚本，分为攻击方和被击方
    {
        int damage = Mathf.Max(attacker.CurrentDamage() - defener.CurrentDefence,1);//攻击力减去防御力,最低是一点伤害
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);//当前血量最低为0
        if(attacker.isCritical)//攻击方暴击了
        {
            defener.GetComponent<Animator>().SetTrigger("Hit");//获取被击方动画组件,启动受伤动画
        }
        //Update UI
        UpdataHealthBarOnAttack?.Invoke(CurrentHealth, MaxHealth);//血条更新，返回现在的血量和最大血量
        //经验Update
        if (CurrentHealth <= 0)
            attacker.characterData.UpdateExp(characterData.KillPoint);//击杀之后把经验加到击杀者身上
    }
   
    public void TakeDamage(int damage,CharacterStats defener)//重载的伤害函数
    {
        int currentDamage = Mathf.Max(damage - defener.CurrentDefence, 0);//伤害最少大于1
        CurrentHealth = Mathf.Max(CurrentHealth - currentDamage, 0);//减血最小不为负

        UpdataHealthBarOnAttack?.Invoke(CurrentHealth, MaxHealth);//血条更新，返回现在的血量和最大血量

        if (CurrentHealth <= 0)
            GameManager.Instance.playerStats.characterData.UpdateExp(characterData.KillPoint);
    }

    private int CurrentDamage()//当前伤害，最小和最大波动
    {
        float coreDamage = UnityEngine.Random.Range(attackData.minDamage, attackData.maxDamage);//min-max之间波动
        //如果暴击
        if(isCritical)
        {
            coreDamage *= attackData.criticalMultiplier;
            //Debug.Log("暴击了"+ coreDamage);
        }    

        return (int)coreDamage;//返回伤害
    }
    #endregion

    #region Equip Weapon

    public void ChangeWeapon(ItemData_SO weapon)
    {
        UnEquipWeapon();//卸下武器
        EquipWeapon(weapon);//装备武器
    }
    public void EquipWeapon(ItemData_SO weapon)
    {
        //生成预制体
        if (weapon.weaponPrefab != null)
            Instantiate(weapon.weaponPrefab, weaponSlot);//在武器生成的位置生成预制体
        //更新属性
        //切换动画
        attackData.ApplyWeaponData(weapon.weaponData);
        //InventoryManager.Instance.UpdataStatsText(MaxHealth, attackData.minDamage, attackData.maxDamage);//更新攻击数据
        GetComponent<Animator>().runtimeAnimatorController = weapon.weaponAnimator;//切换武器动画
    }

    public void UnEquipWeapon()//卸下武器
    {
        if(weaponSlot.transform.childCount != 0)
        {
            for(int i = 0;i<weaponSlot.transform.childCount;i++)
            {
                Destroy(weaponSlot.transform.GetChild(i).gameObject);//消除武器
            }
        }
        attackData.ApplyWeaponData(baseAttackData);//还原初始攻击力
        //切换动画
        GetComponent<Animator>().runtimeAnimatorController = baseAnimator;//切换回初始动画
    }
    #endregion

    #region Apply Data Change

    public void ApplyHealth(int amount)
    {
        if(CurrentHealth + amount <= MaxHealth)//如果加了血量之后小于等于最大血量
        {
            CurrentHealth += amount;
        }
        else
        {
            CurrentHealth = MaxHealth;//不能大于最大血量
        }
    }
    #endregion
}
