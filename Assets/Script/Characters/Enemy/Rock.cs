using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rock : MonoBehaviour
{
    public enum RockStates{HitPlayer, HitEnemy, HitNothing}//ʯͷ������״̬ ������ң��������ˣ�û�й���
    private Rigidbody rb;//����

    public RockStates rockStates;//ʯͷ��ǰ״̬

    [Header("Basic Settings")]
    public float force;//��

    public int damage;//ʯͷ���е��˺�

    public GameObject target;//Ŀ��

    private Vector3 direction;

    public GameObject breakEffect;//ʯͷ��Ч

    void Start()
    {
        rb = GetComponent<Rigidbody>();//��ø���
        rb.velocity = Vector3.one;//����һ��1���ٶ�
        rockStates = RockStates.HitPlayer;//��ʼĿ��Ϊ���
        FlyToTarget();
    }

    void FixedUpdate()
    {
        if(rb.velocity.sqrMagnitude < 1f)
        {
            rockStates = RockStates.HitNothing;//���ʯͷ�ٶ�С��1���л�Ϊ��ͨ״̬
        }
    }

    public void FlyToTarget()
    {
        if (target == null)
            target = FindObjectOfType<PlayerController>().gameObject;//�����ʧĿ��Ͱ�Ŀ����Ϊ���
        direction = (target.transform.position - transform.position + Vector3.up).normalized;
        rb.AddForce(direction * force, ForceMode.Impulse);//��Ϊ�����
    }

    void OnCollisionEnter(Collision other)//����������
    {
        switch(rockStates)
        {
            case RockStates.HitPlayer:
                if (other.gameObject.CompareTag("Player"))//�����������
                {
                    other.gameObject.GetComponent<NavMeshAgent>().isStopped = true;//ʹ���ֹͣ
                    other.gameObject.GetComponent<NavMeshAgent>().velocity = direction * force;//����һ����

                    Debug.Log("����");
                    other.gameObject.GetComponent<Animator>().SetTrigger("Dizzy");//�������,���ӣ����˼����򲻹�
                    other.gameObject.GetComponent<CharacterStats>().TakeDamage(damage, other.gameObject.GetComponent<CharacterStats>());//�������һ���˺�

                    rockStates = RockStates.HitNothing;//��Ϊ����״̬
                    Destroy(gameObject, 10f);//ʮ��֮��ʯͷ��ʧ
                }
                break;
            case RockStates.HitEnemy:
                if(other.gameObject.GetComponent<Golem>())//������е���ʯͷ��
                {
                    var otherStats = other.gameObject.GetComponent<CharacterStats>();
                    otherStats.TakeDamage(damage*2,otherStats);//��ʯͷ����������˺�
                    Instantiate(breakEffect, transform.position, Quaternion.identity);//����ʯͷ��Ч
                    Destroy(gameObject);//����ʯͷ
                }
                break;
        }
    }
}
