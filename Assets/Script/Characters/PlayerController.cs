using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private NavMeshAgent agent;

    private Animator anim;//获得动画

    private CharacterStats characterStats;//获取状态数据

    private GameObject attackTarget;//攻击目标

    private float lastAttackTime;//攻击CD时间

    private bool isDead;//死亡判定

    private float stopDistance;//停止时候的攻击距离

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();//获取物体的NavMeshAgent变量
        anim = GetComponent<Animator>();//获取动画
        characterStats = GetComponent<CharacterStats>();//获取状态

        stopDistance = agent.stoppingDistance;//获取移动时的攻击距离
    }

    void OnEnable()
    {
        //使用委托
        MouseManager.Instance.OnMouseClicked += MoveToTarget;//将方法加到函数
        MouseManager.Instance.OnEnemyClicked += EventAttack;//添加移动到攻击目标
        GameManager.Instance.RigisterPlayer(characterStats);//创建单例玩家

    }

    void Start()
    {
        SaveManager.Instance.LoadPlayerData();//读取存档->这里如果没有存档拿的就是一个新存档
    }

    void OnDisable()//人物消除之后
    {
        MouseManager.Instance.OnMouseClicked -= MoveToTarget;//移除移动目标
        MouseManager.Instance.OnEnemyClicked -= EventAttack;//移除点击攻击
    }


    void Update()
    {
        isDead = characterStats.CurrentHealth == 0;//血量为0时判定死亡
        if (isDead)
            GameManager.Instance.NotifyObservers();//执行广播，所有敌人执行结束动作
        SwitchAnimation();

        lastAttackTime -= Time.deltaTime;//冷却减少
    }

    //改变速度
    private void SwitchAnimation()//变化动作
    {
        anim.SetFloat("Speed", agent.velocity.sqrMagnitude);//sqr是让agent转化为vector3变量，根据速度改变动作
        anim.SetBool("Death", isDead);
    }

    //当游戏开始时候将方法注册到mouseManager
    public void MoveToTarget(Vector3 target)//移动到点
    {
        StopAllCoroutines();//打断移动去攻击目标
        if (isDead) return;
        agent.stoppingDistance = stopDistance;//日常移动为普通点击范围
        agent.isStopped = false;//重置可移动状态
        agent.destination = target;//移动角色
    }

    private void EventAttack(GameObject target)//移动到哪里去攻击
    {
        if (isDead) return;//当死亡时不再能操作
        if (target!=null)//目标是否为空
        {
            attackTarget = target;//点击的目标为攻击目标
            characterStats.isCritical = UnityEngine.Random.value < characterStats.attackData.criticalChance;
            StartCoroutine(MoveToAttackTarget());//执行移动攻击代码
        }
    }

    IEnumerator MoveToAttackTarget()
    {
        agent.isStopped = false;//不停止
        agent.stoppingDistance = characterStats.attackData.attackRange;//将移动时的攻击距离修改为角色的攻击距离

        transform.LookAt(attackTarget.transform);//转向目标

        while(Vector3.Distance(attackTarget.transform.position,transform.position) > characterStats.attackData.attackRange)//如果两者距离小于1则将目标转换成攻击目标的位置
        {
            agent.destination = attackTarget.transform.position;
            yield return null;
        }

        agent.isStopped = true;//停止动作
        //Attack
        if(lastAttackTime < 0 )//没有进入冷却时间
        {
            anim.SetBool("Critical", characterStats.isCritical);//暴击
            anim.SetTrigger("Attack");//启动攻击动画
            //重置冷却时间
            lastAttackTime = characterStats.attackData.coolDown;
        }
    }

    //Animation Event
    void Hit()
    {
        if(attackTarget.CompareTag("Attackable"))//如果攻击的是石头等
        {
            if (attackTarget.GetComponent<Rock>()&&attackTarget.GetComponent<Rock>().rockStates == Rock.RockStates.HitNothing)//如果点击的是石头而且落地了，切换石头状态
            {
                attackTarget.GetComponent<Rock>().rockStates = Rock.RockStates.HitEnemy;//修改石头状态
                attackTarget.GetComponent<Rigidbody>().velocity = Vector3.one;//给予初始1的速度
                attackTarget.GetComponent<Rigidbody>().AddForce(transform.forward * 20,ForceMode.Impulse);//给予一个力 速度为20
            }
        }
        else
        {
            var targetStats = attackTarget.GetComponent<CharacterStats>();//获取攻击方

            targetStats.TakeDamage(characterStats, targetStats);//分别为攻击方和防守方
        }
        
            
        
    }

}
