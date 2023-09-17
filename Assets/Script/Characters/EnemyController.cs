using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyStates { GUARD,PATROL,CHASE,DEAD}//怪物状态枚举变量
[RequireComponent(typeof(NavMeshAgent))]//自动加上NavMeshAgent
[RequireComponent(typeof(CharacterStats))]//自动加上CharacterStats
public class EnemyController : MonoBehaviour,IEndGameObserver
{
    private EnemyStates enemyStates;//怪物状态

    private NavMeshAgent agent;//移动目标

    private Animator anim;//动画

    private Collider coll;//状态机

    protected CharacterStats characterStats;

    [Header("Basic Settings")]//基础属性，是否巡逻可视范围追逐范围

    public float sightRadius;//可视范围

    public bool isGuard;//守卫状态

    private float speed;//追逐速度

    protected GameObject attackTarget;//攻击目标

    public float LookAtTime;//观察时间

    private float remainLookAtTime;//剩余观察时间

    private float LastAttackTime;//现在的冷却时间

    private Quaternion guardRotation;//初始角度

    [Header("Patrol State")]
    public float patrolRange;//活动范围

    private Vector3 wayPoint;//巡逻点

    private Vector3 guardPos;//初始位置

    //状态机
    bool isWalk;//移动

    bool isChase;//追逐

    bool isFollow;//跟随

    bool isDead;//死亡

    bool playerDead;//玩家死亡

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();//获取目标
        anim = GetComponent<Animator>();//动画机
        characterStats = GetComponent<CharacterStats>();//初始状态
        coll = GetComponent<Collider>();//控制器

        speed = agent.speed;//移动速度
        guardPos = transform.position;//初始位置
        guardRotation = transform.rotation;//初始角度
        remainLookAtTime = LookAtTime;//剩余观察时间
    }

    void OnDisable()
    {
        if (!GameManager.IsInitialized) return;//如果不在列表中则不管
        GameManager.Instance.RemoveObserver(this);

        if(GetComponent<LootSpawner>()&&isDead)//怪物死亡则删除怪物
        {
            GetComponent<LootSpawner>().SpawnLoot();//ִ删除怪物
        }

        if(QuestManager.IsInitialized &&isDead)
        {
            QuestManager.Instance.UpdateQuestProgress(this.name, 1);//将列表中删除一个这个变量
        }
    }

    void Start()
    {
        if(isGuard)
        {
            enemyStates = EnemyStates.GUARD;//初始为守卫状态
        }
        else
        {
            enemyStates = EnemyStates.PATROL;//初始为巡逻状态
            GetNewWayPoint();
        }
        GameManager.Instance.AddObserver(this);

    }

    void Update()
    {
        if (characterStats.CurrentHealth == 0)
            isDead = true;//血量为0则死亡

        if (!playerDead)//玩家没死
        {
            SwitchStates();//赋予状态
            SwitchAnimation();//赋予动画

            LastAttackTime -= Time.deltaTime;//减少冷却时间
        }

    }


    void SwitchAnimation()//赋予状态机
    {
        anim.SetBool("Walk", isWalk);
        anim.SetBool("Chase", isChase);
        anim.SetBool("Follow", isFollow);
        anim.SetBool("Critical", characterStats.isCritical);
        anim.SetBool("Death", isDead);
    }

    void SwitchStates()//初始化
    {
        if (isDead)//如果怪物死亡则切换死亡状态
            enemyStates = EnemyStates.DEAD;
        //发现玩家
        else if(FoundPlayer())
        {
            enemyStates = EnemyStates.CHASE;//切换追逐状态
            //Debug.Log("�ҵ����");
        }
        switch(enemyStates)
        {
            case EnemyStates.GUARD:
                isChase = false;

                if(transform.position != guardPos)
                {
                    isWalk = true;//变成走路
                    agent.isStopped = false;
                    agent.destination = guardPos;//目标为初始位置

                    if (Vector3.SqrMagnitude(guardPos - transform.position) <= agent.stoppingDistance)//如果坐标回来了就转头
                    {
                        isWalk = false;
                        //Debug.Log("回了？");
                        transform.rotation = Quaternion.Lerp(transform.rotation,guardRotation,0.01f);//转头
                    }
                        
                }
                break;
            case EnemyStates.PATROL:
                isChase = false;
                agent.speed = speed * 0.5f;//巡逻状态为初始速度的0.5倍
                //如果丢失玩家
                if(Vector3.Distance(wayPoint,transform.position) <= agent.stoppingDistance)//还没走到随机点
                {
                    isWalk = false;
                    if(remainLookAtTime > 0)
                    {
                        remainLookAtTime -= Time.deltaTime;
                    }
                    else
                        GetNewWayPoint();//获取新随机点
                }
                else
                {
                    isWalk = true;

                    agent.destination = wayPoint;
                }
                break;
            case EnemyStates.CHASE:

                isWalk = false;
                isChase = true;

                agent.speed = speed;//初始化速度
                //追逐丢失玩家
                if(!FoundPlayer())//丢失玩家
                {
                    //回复原状态
                    isFollow = false;
                    if (remainLookAtTime > 0)
                    {
                        agent.destination = transform.position;//目标点位现在这个点
                        remainLookAtTime -= Time.deltaTime;
                    }
                    //回到原来的状态
                    else if (isGuard)
                        enemyStates = EnemyStates.GUARD;
                    else
                        enemyStates = EnemyStates.PATROL;
                    
                }
                else
                {
                    remainLookAtTime = LookAtTime;
                    isFollow = true;
                    agent.isStopped = false;
                    agent.destination = attackTarget.transform.position;//目标点位玩家的坐标
                }

                //进入攻击范围
                if(TargetInAttackRange()||TargetInSkillkRange())//如果在攻击和技能范围内
                {
                    isFollow = false;//ֹͣ׷��
                    agent.isStopped = true;//ֹͣ

                    if(LastAttackTime < 0 )
                    {
                        LastAttackTime = characterStats.attackData.coolDown;//增加冷却时间

                        //判断暴击
                        characterStats.isCritical = Random.value < characterStats.attackData.criticalChance;//如果出现暴击
                        //ִ执行攻击
                        Attack();
                    }
                }
                break;
            case EnemyStates.DEAD:
                coll.enabled = false;
                //agent.enabled = false;
                agent.radius = 0;//��С�ɵ����Χ
                Destroy(gameObject, 2f);//�����ɾ��
                break;
        }
    }

    void Attack()
    {
        transform.LookAt(attackTarget.transform);//�������
        if(TargetInAttackRange())
        {
            //���������Χ��
            anim.SetTrigger("Attack");
        }
        if(TargetInSkillkRange())
        {
            //Զ�̹�����Χ��
            anim.SetTrigger("Skill");
        }
    }

    bool FoundPlayer()//�ж��Ƿ������
    {
        var colliders = Physics.OverlapSphere(transform.position, sightRadius);//�����ӷ�Χ�ڵ����ж�������¼

        foreach(var target in colliders)
        {
            if(target.CompareTag("Player"))//�ҵ����
            {
                attackTarget = target.gameObject;//�ҵ���Ҿ͸�ֵ
                return true;
            }
        }
        attackTarget = null;//û�ҵ����
        return false;//û���ҵ�

    }

    bool TargetInAttackRange()//������Χ��
    {
        if (attackTarget != null)
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterStats.attackData.attackRange;
        else
            return false;
    }

    bool TargetInSkillkRange()//���ܷ�Χ��
    {
        if (attackTarget != null)
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterStats.attackData.skillRange;
        else
            return false;
    }

    void GetNewWayPoint()//��ȡѲ�ߵ�
    {
        //���˹۲�ʱ��
        remainLookAtTime = LookAtTime;
        //����ֻ���ڷ�Χ�ڵ�xzֵ
        float randomX = Random.Range(-patrolRange, patrolRange);
        float randomZ = Random.Range(-patrolRange, patrolRange);

        Vector3 randomPoint = new Vector3(guardPos.x + randomX, transform.position.y, guardPos.z + randomZ);//��ȡ�µ�

        //�ܹ������߷�Χ
        NavMeshHit hit;
        wayPoint = NavMesh.SamplePosition(randomPoint, out hit, patrolRange, 1) ?  hit.position : transform.position;//������Ѳ�߷�Χ���ж����ҵĵ��Ƿ��ײ��


    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;//�۲���Ϊ��ɫ
        Gizmos.DrawWireSphere(transform.position, sightRadius);//�ӿ��ӷ�Χ��Ȧ
    }

    //Animation Event
    void Hit()
    {
        if(attackTarget != null&&transform.IsFacingTarget(attackTarget.transform))//�ڶ���ǿյ����,������cos45������
        {
            var targetStats = attackTarget.GetComponent<CharacterStats>();
            targetStats.TakeDamage(characterStats, targetStats);
        }
    }

    public void EndNotify()
    {
        //广播模式，玩家死亡时产生
        anim.SetBool("Win", true);//ʤ������
        playerDead = true;//�������
        isChase = false;
        isWalk = false;
        attackTarget = null;//ʧȥĿ��
    }
}
