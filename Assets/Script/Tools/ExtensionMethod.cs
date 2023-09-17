using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethod
{
    //左右45°角内才能攻击
    private const float dotThreshold = 0.5f;//固定值cos60° = 0.5 
    public static bool IsFacingTarget(this Transform transform,Transform target)//判断目标是否在前面 本身与变量
    {

        var vectorToTarget = target.position - transform.position;//两者相距
        vectorToTarget.Normalize();

        float dot = Vector3.Dot(transform.forward, vectorToTarget);//获取两者间的cos值

        return dot >= dotThreshold;

    }
}
