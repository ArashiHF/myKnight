using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethod
{
    //����45����ڲ��ܹ���
    private const float dotThreshold = 0.5f;//�̶�ֵcos60�� = 0.5 
    public static bool IsFacingTarget(this Transform transform,Transform target)//�ж�Ŀ���Ƿ���ǰ�� ���������
    {

        var vectorToTarget = target.position - transform.position;//�������
        vectorToTarget.Normalize();

        float dot = Vector3.Dot(transform.forward, vectorToTarget);//��ȡ���߼��cosֵ

        return dot >= dotThreshold;

    }
}
