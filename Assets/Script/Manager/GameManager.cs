using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : Singleton<GameManager>
{
    public CharacterStats playerStats;//玩家状态

    private CinemachineFreeLook followCamera;//移动相机

    List<IEndGameObserver> endGameObservers = new List<IEndGameObserver>();//作为一个列表用于添加所有在场景中的敌人

    protected override void Awake()//不删除角色
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
    public void RigisterPlayer(CharacterStats player)//获取玩家状态
    {
        playerStats = player;

        followCamera = FindObjectOfType<CinemachineFreeLook>();

        if (followCamera != null)
        {
            followCamera.Follow = playerStats.transform.GetChild(2);
            followCamera.LookAt = playerStats.transform.GetChild(2);
        }

    }

    public void AddObserver(IEndGameObserver observer)//添加入观察者列表
    {
        endGameObservers.Add(observer);//添加
    }

    public void RemoveObserver(IEndGameObserver observer)//在观察者列表删除
    {
        endGameObservers.Remove(observer);//删除
    }

    public void NotifyObservers()
    {
        foreach (var observer in endGameObservers)//所有列表中的角色都执行动作
        {
            observer.EndNotify();
        }
    }

    public Transform GetEntrance()
    {
        foreach(var item in FindObjectsOfType<TransitionDestination>())
        {
            if (item.destinationTag == TransitionDestination.DestinationTag.ENTER)
                return item.transform;//返回目标
        }
        return null;
    }
}
