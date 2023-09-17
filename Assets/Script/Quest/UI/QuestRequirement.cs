using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestRequirement : MonoBehaviour
{
    private Text requireName;//需求名字

    private Text progresNumber;//需求数量

    void Awake()
    {
        requireName = GetComponent<Text>();
        progresNumber = transform.GetChild(0).GetComponent<Text>();
    }

    public void SetupRequirement(string name,int amount,int currentAmount)
    {
        requireName.text = name;//更新任务要求名字
        progresNumber.text = currentAmount.ToString() + "/" + amount.ToString();//更新任务要求数量
    }

    public void SetupRequirement(string name,bool isFinished)//完成任务之后重载
    {
        if(isFinished)
        {
            requireName.text = name;//更新任务要求名字
            progresNumber.text = "Finish";//完成任务时直接把数量变成完成
            requireName.color = Color.gray;//名字变灰
            progresNumber.color = Color.gray;//要求也变灰

        }
    }
}
