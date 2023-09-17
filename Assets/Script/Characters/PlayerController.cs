using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private NavMeshAgent agent;

    private Animator anim;//��ö���

    private CharacterStats characterStats;//��ȡ״̬����

    private GameObject attackTarget;//����Ŀ��

    private float lastAttackTime;//����CDʱ��

    private bool isDead;//�����ж�

    private float stopDistance;//ֹͣʱ��Ĺ�������

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();//��ȡ�����NavMeshAgent����
        anim = GetComponent<Animator>();//��ȡ����
        characterStats = GetComponent<CharacterStats>();//��ȡ״̬

        stopDistance = agent.stoppingDistance;//��ȡ�ƶ�ʱ�Ĺ�������
    }

    void OnEnable()
    {
        //ʹ��ί��
        MouseManager.Instance.OnMouseClicked += MoveToTarget;//�������ӵ�����
        MouseManager.Instance.OnEnemyClicked += EventAttack;//����ƶ�������Ŀ��
        GameManager.Instance.RigisterPlayer(characterStats);//�����������

    }

    void Start()
    {
        SaveManager.Instance.LoadPlayerData();//��ȡ�浵->�������û�д浵�õľ���һ���´浵
    }

    void OnDisable()//��������֮��
    {
        MouseManager.Instance.OnMouseClicked -= MoveToTarget;//�Ƴ��ƶ�Ŀ��
        MouseManager.Instance.OnEnemyClicked -= EventAttack;//�Ƴ��������
    }


    void Update()
    {
        isDead = characterStats.CurrentHealth == 0;//Ѫ��Ϊ0ʱ�ж�����
        if (isDead)
            GameManager.Instance.NotifyObservers();//ִ�й㲥�����е���ִ�н�������
        SwitchAnimation();

        lastAttackTime -= Time.deltaTime;//��ȴ����
    }

    //�ı��ٶ�
    private void SwitchAnimation()//�仯����
    {
        anim.SetFloat("Speed", agent.velocity.sqrMagnitude);//sqr����agentת��Ϊvector3�����������ٶȸı䶯��
        anim.SetBool("Death", isDead);
    }

    //����Ϸ��ʼʱ�򽫷���ע�ᵽmouseManager
    public void MoveToTarget(Vector3 target)//�ƶ�����
    {
        StopAllCoroutines();//����ƶ�ȥ����Ŀ��
        if (isDead) return;
        agent.stoppingDistance = stopDistance;//�ճ��ƶ�Ϊ��ͨ�����Χ
        agent.isStopped = false;//���ÿ��ƶ�״̬
        agent.destination = target;//�ƶ���ɫ
    }

    private void EventAttack(GameObject target)//�ƶ�������ȥ����
    {
        if (isDead) return;//������ʱ�����ܲ���
        if (target!=null)//Ŀ���Ƿ�Ϊ��
        {
            attackTarget = target;//�����Ŀ��Ϊ����Ŀ��
            characterStats.isCritical = UnityEngine.Random.value < characterStats.attackData.criticalChance;
            StartCoroutine(MoveToAttackTarget());//ִ���ƶ���������
        }
    }

    IEnumerator MoveToAttackTarget()
    {
        agent.isStopped = false;//��ֹͣ
        agent.stoppingDistance = characterStats.attackData.attackRange;//���ƶ�ʱ�Ĺ��������޸�Ϊ��ɫ�Ĺ�������

        transform.LookAt(attackTarget.transform);//ת��Ŀ��

        while(Vector3.Distance(attackTarget.transform.position,transform.position) > characterStats.attackData.attackRange)//������߾���С��1��Ŀ��ת���ɹ���Ŀ���λ��
        {
            agent.destination = attackTarget.transform.position;
            yield return null;
        }

        agent.isStopped = true;//ֹͣ����
        //Attack
        if(lastAttackTime < 0 )//û�н�����ȴʱ��
        {
            anim.SetBool("Critical", characterStats.isCritical);//����
            anim.SetTrigger("Attack");//������������
            //������ȴʱ��
            lastAttackTime = characterStats.attackData.coolDown;
        }
    }

    //Animation Event
    void Hit()
    {
        if(attackTarget.CompareTag("Attackable"))//�����������ʯͷ��
        {
            if (attackTarget.GetComponent<Rock>()&&attackTarget.GetComponent<Rock>().rockStates == Rock.RockStates.HitNothing)//����������ʯͷ��������ˣ��л�ʯͷ״̬
            {
                attackTarget.GetComponent<Rock>().rockStates = Rock.RockStates.HitEnemy;//�޸�ʯͷ״̬
                attackTarget.GetComponent<Rigidbody>().velocity = Vector3.one;//�����ʼ1���ٶ�
                attackTarget.GetComponent<Rigidbody>().AddForce(transform.forward * 20,ForceMode.Impulse);//����һ���� �ٶ�Ϊ20
            }
        }
        else
        {
            var targetStats = attackTarget.GetComponent<CharacterStats>();//��ȡ������

            targetStats.TakeDamage(characterStats, targetStats);//�ֱ�Ϊ�������ͷ��ط�
        }
        
            
        
    }

}
