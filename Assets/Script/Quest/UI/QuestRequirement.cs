using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestRequirement : MonoBehaviour
{
    private Text requireName;//��������

    private Text progresNumber;//��������

    void Awake()
    {
        requireName = GetComponent<Text>();
        progresNumber = transform.GetChild(0).GetComponent<Text>();
    }

    public void SetupRequirement(string name,int amount,int currentAmount)
    {
        requireName.text = name;//��������Ҫ������
        progresNumber.text = currentAmount.ToString() + "/" + amount.ToString();//��������Ҫ������
    }

    public void SetupRequirement(string name,bool isFinished)//�������֮������
    {
        if(isFinished)
        {
            requireName.text = name;//��������Ҫ������
            progresNumber.text = "Finish";//�������ʱֱ�Ӱ�����������
            requireName.color = Color.gray;//���ֱ��
            progresNumber.color = Color.gray;//Ҫ��Ҳ���

        }
    }
}
