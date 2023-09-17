using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;//��������

    public static T Instance
    {
        get { return instance; }//�������ɱ���
    }

    protected virtual void Awake()//ֻ����̳������
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = (T)this;//����Ϊ�յ�ʱ������Ϊ�̳���
    }

    public static bool IsInitialized
    {
        get { return instance != null; }//���ص�ǰ���͵����Ƿ�����true/false
    }

    protected virtual void OnDestroy()//����е����Ļ���ɾ�������
    {
        if(instance == this)
        {
            instance = null;
        }
    }

}
