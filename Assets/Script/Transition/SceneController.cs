using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class SceneController : Singleton<SceneController>,IEndGameObserver
{
    public GameObject playerPrefab;//���Ԥ����

    public SceneFader sceneFaderPrefab;//����Ԥ����

    GameObject player;//���ص����

    NavMeshAgent playerAgent;//������

    bool fadeFinished = true;

    protected override void Awake()//��ɾ����ɫ
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        GameManager.Instance.AddObserver(this);
        fadeFinished = true;
    }
    public void TransitionToDestination(TransitionPoint transitionPoint)
    {
        // SaveManager.Instance.SavePlayerData();//�����������
        // InventoryManager.Instance.SaveData();//���汳������
        // QuestManager.Instance.SaveQuestManager();//������������
        switch(transitionPoint.transitionType)
        {
            case TransitionPoint.TransitionType.SameScene://��ͬ��������
                StartCoroutine(Transition(SceneManager.GetActiveScene().name, transitionPoint.destinationTag));//��ȡת�����������ֺ�����,ִ��ת��
                break;
            case TransitionPoint.TransitionType.DifferentScene://��ͬ��������
                StartCoroutine(Transition(transitionPoint.sceneName, transitionPoint.destinationTag));//��ͬ��������
                break;
        }
    }

    IEnumerator Transition(string sceneName, TransitionDestination.DestinationTag destinationTag)//�첽����
    {
        //����Ҫ��������
        SaveManager.Instance.SavePlayerData();//�����������
        InventoryManager.Instance.SaveData();//���汳������
        QuestManager.Instance.SaveQuestManager();//������������
        if(SceneManager.GetActiveScene().name!=sceneName)//��ͬ����
        {
            yield return SceneManager.LoadSceneAsync(sceneName);//�첽���ض�Ӧ����
            yield return Instantiate(playerPrefab, GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);//��destination���ɽ�ɫ
            SaveManager.Instance.LoadPlayerData();//��ȡ�������
            yield break;
        }
        else//��ͬ����
        {
            player = GameManager.Instance.playerStats.gameObject;//��ȡ���
            playerAgent = player.GetComponent<NavMeshAgent>();
            playerAgent.enabled = false;//ֹͣ�ƶ�
            player.transform.SetPositionAndRotation(GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);//������Ҫ��tagλ�úͷ���
            playerAgent.enabled = true;//���ƶ�
            yield return null;
        }
    }

    private TransitionDestination GetDestination(TransitionDestination.DestinationTag destinationTag)//�ҵ������з��ϵ�destination
    {
        var entrances = FindObjectsOfType<TransitionDestination>();
        for(int i = 0;i<entrances.Length;i++)
        {
            if (entrances[i].destinationTag == destinationTag)
                return entrances[i];//�ҵ���Ҫ�ĳ�����tag�Ļ��򷵻�
        }

        return null;
    }

    public void TranitionToMain()//����������
    {
        StartCoroutine(LoadMain());
    }

    public void TransitionToLoadGame()//��ȡ�浵
    {
        StartCoroutine(LoadLevel(SaveManager.Instance.SceneName));
    }

    public void TransitionToFirstLevel()//���ص�һ������,����Ϸ��ʹ��
    {
        StartCoroutine(LoadLevel("Game"));
    }

    IEnumerator LoadLevel(string scene)
    {
        SceneFader fade = Instantiate(sceneFaderPrefab);//����Ԥ�������
        if(scene != "")
        {
            //����
            yield return StartCoroutine(fade.FadeOut(2f));

            yield return SceneManager.LoadSceneAsync(scene);
            yield return player = Instantiate(playerPrefab,GameManager.Instance.GetEntrance().position,GameManager.Instance.GetEntrance().rotation);//�������Ԥ����λ����Ƕ�
        }
        //��������
        SaveManager.Instance.SavePlayerData();//�������
        InventoryManager.Instance.SaveData();//��������

        //����
        yield return StartCoroutine(fade.FadeIn(2f));

        yield break;
    }

    IEnumerator LoadMain()
    {
        SceneFader fade = Instantiate(sceneFaderPrefab);//����Ԥ�������

        yield return StartCoroutine(fade.FadeOut(2f));

        yield return SceneManager.LoadSceneAsync("Main");//���ز˵�ҳ��

        yield return StartCoroutine(fade.FadeIn(2f));
        yield break;
    }

    public void EndNotify()
    {
        if(fadeFinished)
        {
            fadeFinished = false;
            StartCoroutine(LoadMain());//�������ʱ��ص����˵�
        }
        
    }
}
