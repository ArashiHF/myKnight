using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Grunt : EnemyController
{
    [Header("Skill")]
    public float kickForce = 10;

    public void KickOff()//�ƺ�
    {
        if(attackTarget != null)//�������Ŀ�겻Ϊ��
        {
            transform.LookAt(attackTarget.transform);//�������

            Vector3 direction = attackTarget.transform.position - transform.position;//�����ķ���

            direction.Normalize();//0 1 -1��ѡȡ

            attackTarget.GetComponent<NavMeshAgent>().isStopped = true;//����ƶ�
            attackTarget.GetComponent<NavMeshAgent>().velocity = direction * kickForce;//����һ���������

            attackTarget.GetComponent<Animator>().SetTrigger("Dizzy");

        }
    }
}
