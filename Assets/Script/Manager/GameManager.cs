using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : Singleton<GameManager>
{
    public CharacterStats playerStats;//���״̬

    private CinemachineFreeLook followCamera;//�ƶ����

    List<IEndGameObserver> endGameObservers = new List<IEndGameObserver>();//��Ϊһ���б�������������ڳ����еĵ���

    protected override void Awake()//��ɾ����ɫ
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
    public void RigisterPlayer(CharacterStats player)//��ȡ���״̬
    {
        playerStats = player;

        followCamera = FindObjectOfType<CinemachineFreeLook>();

        if (followCamera != null)
        {
            followCamera.Follow = playerStats.transform.GetChild(2);
            followCamera.LookAt = playerStats.transform.GetChild(2);
        }

    }

    public void AddObserver(IEndGameObserver observer)//�����۲����б�
    {
        endGameObservers.Add(observer);//���
    }

    public void RemoveObserver(IEndGameObserver observer)//�ڹ۲����б�ɾ��
    {
        endGameObservers.Remove(observer);//ɾ��
    }

    public void NotifyObservers()
    {
        foreach (var observer in endGameObservers)//�����б��еĽ�ɫ��ִ�ж���
        {
            observer.EndNotify();
        }
    }

    public Transform GetEntrance()
    {
        foreach(var item in FindObjectsOfType<TransitionDestination>())
        {
            if (item.destinationTag == TransitionDestination.DestinationTag.ENTER)
                return item.transform;//����Ŀ��
        }
        return null;
    }
}
