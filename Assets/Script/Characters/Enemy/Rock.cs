using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rock : MonoBehaviour
{
    public enum RockStates{HitPlayer, HitEnemy, HitNothing}//石头的三种状态 攻击玩家，攻击敌人，没有攻击
    private Rigidbody rb;//刚体

    public RockStates rockStates;//石头当前状态

    [Header("Basic Settings")]
    public float force;//力

    public int damage;//石头砸中的伤害

    public GameObject target;//目标

    private Vector3 direction;

    public GameObject breakEffect;//石头特效

    void Start()
    {
        rb = GetComponent<Rigidbody>();//获得刚体
        rb.velocity = Vector3.one;//给予一个1的速度
        rockStates = RockStates.HitPlayer;//初始目标为玩家
        FlyToTarget();
    }

    void FixedUpdate()
    {
        if(rb.velocity.sqrMagnitude < 1f)
        {
            rockStates = RockStates.HitNothing;//如果石头速度小于1则切换为普通状态
        }
    }

    public void FlyToTarget()
    {
        if (target == null)
            target = FindObjectOfType<PlayerController>().gameObject;//如果丢失目标就把目标设为玩家
        direction = (target.transform.position - transform.position + Vector3.up).normalized;
        rb.AddForce(direction * force, ForceMode.Impulse);//力为冲击力
    }

    void OnCollisionEnter(Collision other)//当碰到东西
    {
        switch(rockStates)
        {
            case RockStates.HitPlayer:
                if (other.gameObject.CompareTag("Player"))//攻击的是玩家
                {
                    other.gameObject.GetComponent<NavMeshAgent>().isStopped = true;//使玩家停止
                    other.gameObject.GetComponent<NavMeshAgent>().velocity = direction * force;//给予一个力

                    Debug.Log("击退");
                    other.gameObject.GetComponent<Animator>().SetTrigger("Dizzy");//击晕玩家,不加，加了几乎打不过
                    other.gameObject.GetComponent<CharacterStats>().TakeDamage(damage, other.gameObject.GetComponent<CharacterStats>());//给予玩家一倍伤害

                    rockStates = RockStates.HitNothing;//变为正常状态
                    Destroy(gameObject, 10f);//十秒之后石头消失
                }
                break;
            case RockStates.HitEnemy:
                if(other.gameObject.GetComponent<Golem>())//如果命中的是石头人
                {
                    var otherStats = other.gameObject.GetComponent<CharacterStats>();
                    otherStats.TakeDamage(damage*2,otherStats);//对石头人造成两倍伤害
                    Instantiate(breakEffect, transform.position, Quaternion.identity);//生成石头特效
                    Destroy(gameObject);//消除石头
                }
                break;
        }
    }
}
