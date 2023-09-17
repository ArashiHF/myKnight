using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Golem : EnemyController
{
    [Header("Skill")]

    public float kickForce = 25;

    public GameObject rockPrefab;//ʯͷԤ����

    public Transform handPos;//��ʯͷ��ʯͷ���ɵ�λ��
    //Animation Event
    public void KickOff()
    {
        if (attackTarget != null && transform.IsFacingTarget(attackTarget.transform))//�ڶ���ǿյ����,������cos45������,ʹ�õ���hit����
        {
            var targetStats = attackTarget.GetComponent<CharacterStats>();

            Vector3 direction = (attackTarget.transform.position - transform.position).normalized;//���跽��

            targetStats.GetComponent<NavMeshAgent>().isStopped = true;//ֹͣ
            targetStats.GetComponent<NavMeshAgent>().velocity = direction * kickForce;//�������
            Debug.Log("玩家眩晕");
            targetStats.GetComponent<Animator>().SetTrigger("Dizzy");//���ѣ��
            targetStats.TakeDamage(characterStats, targetStats);//����˺�
        }
    }

    //Animation Event Rock
    public void ThrowRock()//��ʯͷ
    {
        if(attackTarget!=null)
        {
            var rock = Instantiate(rockPrefab, handPos.position, Quaternion.identity);//����ʯͷ��������������ͷ���
            rock.GetComponent<Rock>().target = attackTarget;//����Ŀ��
        }
    }
}
