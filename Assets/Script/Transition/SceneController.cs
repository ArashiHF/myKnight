using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class SceneController : Singleton<SceneController>,IEndGameObserver
{
    public GameObject playerPrefab;//玩家预制体

    public SceneFader sceneFaderPrefab;//渐变预制体

    GameObject player;//加载的玩家

    NavMeshAgent playerAgent;//控制器

    bool fadeFinished = true;

    protected override void Awake()//不删除角色
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
        // SaveManager.Instance.SavePlayerData();//保存玩家数据
        // InventoryManager.Instance.SaveData();//保存背包数据
        // QuestManager.Instance.SaveQuestManager();//保存任务数据
        switch(transitionPoint.transitionType)
        {
            case TransitionPoint.TransitionType.SameScene://相同场景传送
                StartCoroutine(Transition(SceneManager.GetActiveScene().name, transitionPoint.destinationTag));//获取转换场景的名字和类型,执行转移
                break;
            case TransitionPoint.TransitionType.DifferentScene://不同场景传送
                StartCoroutine(Transition(transitionPoint.sceneName, transitionPoint.destinationTag));//不同场景传送
                break;
        }
    }

    IEnumerator Transition(string sceneName, TransitionDestination.DestinationTag destinationTag)//异步加载
    {
        //还需要保存数据
        SaveManager.Instance.SavePlayerData();//保存玩家数据
        InventoryManager.Instance.SaveData();//保存背包数据
        QuestManager.Instance.SaveQuestManager();//保存任务数据
        if(SceneManager.GetActiveScene().name!=sceneName)//不同场景
        {
            yield return SceneManager.LoadSceneAsync(sceneName);//异步加载对应场景
            yield return Instantiate(playerPrefab, GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);//在destination生成角色
            SaveManager.Instance.LoadPlayerData();//读取玩家数据
            yield break;
        }
        else//不同场景
        {
            player = GameManager.Instance.playerStats.gameObject;//获取玩家
            playerAgent = player.GetComponent<NavMeshAgent>();
            playerAgent.enabled = false;//停止移动
            player.transform.SetPositionAndRotation(GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);//传入需要的tag位置和反向
            playerAgent.enabled = true;//可移动
            yield return null;
        }
    }

    private TransitionDestination GetDestination(TransitionDestination.DestinationTag destinationTag)//找到场景中符合的destination
    {
        var entrances = FindObjectsOfType<TransitionDestination>();
        for(int i = 0;i<entrances.Length;i++)
        {
            if (entrances[i].destinationTag == destinationTag)
                return entrances[i];//找到需要的场景的tag的话则返回
        }

        return null;
    }

    public void TranitionToMain()//加载主场景
    {
        StartCoroutine(LoadMain());
    }

    public void TransitionToLoadGame()//读取存档
    {
        StartCoroutine(LoadLevel(SaveManager.Instance.SceneName));
    }

    public void TransitionToFirstLevel()//加载第一个场景,新游戏是使用
    {
        StartCoroutine(LoadLevel("Game"));
    }

    IEnumerator LoadLevel(string scene)
    {
        SceneFader fade = Instantiate(sceneFaderPrefab);//创建预制体变量
        if(scene != "")
        {
            //渐出
            yield return StartCoroutine(fade.FadeOut(2f));

            yield return SceneManager.LoadSceneAsync(scene);
            yield return player = Instantiate(playerPrefab,GameManager.Instance.GetEntrance().position,GameManager.Instance.GetEntrance().rotation);//生成玩家预制体位置与角度
        }
        //保存数据
        SaveManager.Instance.SavePlayerData();//玩家数据
        InventoryManager.Instance.SaveData();//背包数据

        //渐入
        yield return StartCoroutine(fade.FadeIn(2f));

        yield break;
    }

    IEnumerator LoadMain()
    {
        SceneFader fade = Instantiate(sceneFaderPrefab);//创建预制体变量

        yield return StartCoroutine(fade.FadeOut(2f));

        yield return SceneManager.LoadSceneAsync("Main");//加载菜单页面

        yield return StartCoroutine(fade.FadeIn(2f));
        yield break;
    }

    public void EndNotify()
    {
        if(fadeFinished)
        {
            fadeFinished = false;
            StartCoroutine(LoadMain());//玩家死亡时候回到主菜单
        }
        
    }
}
