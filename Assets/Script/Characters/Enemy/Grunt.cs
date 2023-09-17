using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Grunt : EnemyController
{
    [Header("Skill")]
    public float kickForce = 10;

    public void KickOff()//推后
    {
        if(attackTarget != null)//如果攻击目标不为空
        {
            transform.LookAt(attackTarget.transform);//看向玩家

            Vector3 direction = attackTarget.transform.position - transform.position;//推力的方向

            direction.Normalize();//0 1 -1中选取

            attackTarget.GetComponent<NavMeshAgent>().isStopped = true;//打断移动
            attackTarget.GetComponent<NavMeshAgent>().velocity = direction * kickForce;//给予一个方向的力

            attackTarget.GetComponent<Animator>().SetTrigger("Dizzy");

        }
    }
}
