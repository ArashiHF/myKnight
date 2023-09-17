using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionPoint : MonoBehaviour
{
    public enum TransitionType//�������ͣ�ͨ�����Ͳ�ͬ����
    {
        SameScene,DifferentScene
    }

    [Header("Transition Info")]
    public string sceneName;

    public TransitionType transitionType;//��������

    public TransitionDestination.DestinationTag destinationTag;//����Ŀ�ĵ�

    private bool canTrans;//����Trigger

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E)&&canTrans)//������E�����ڿɴ���λ��
        {
            //���д���
            SceneController.Instance.TransitionToDestination(this);
        }
    }

    void OnTriggerStay(Collider other)//��ҿ��������žͿɴ���
    {
        if (other.CompareTag("Player"))
            canTrans = true;
    }
    void OnTriggerExit(Collider other)//����뿪������,�͹رտɴ���
    {
        if (other.CompareTag("Player"))
            canTrans = false;
    }
}
