using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;//创建单例

    public static T Instance
    {
        get { return instance; }//访问自由变量
    }

    protected virtual void Awake()//只允许继承类访问
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = (T)this;//当他为空的时候赋予其为继承类
    }

    public static bool IsInitialized
    {
        get { return instance != null; }//返回当前泛型单例是否生成true/false
    }

    protected virtual void OnDestroy()//如果有单例的话就删掉多余的
    {
        if(instance == this)
        {
            instance = null;
        }
    }

}
