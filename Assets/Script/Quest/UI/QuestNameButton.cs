using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestNameButton : MonoBehaviour
{
    public Text questNameText;//任务按钮名字

    public QuestData_SO currentData;//当前任务

    public Text QuestContentText;//内容文本

    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(UpdateQuestContent);//添加点击之后出现任务内容
    }

    void UpdateQuestContent()//更新任务内容的所有信息
    {
        //QuestContentText.text = currentData.description;//更新文本内容
        QuestUI.Instance.SetupRequireList(currentData);//更新任务要求的内容

        foreach(Transform item in QuestUI.Instance.rewardTransform)
        {
            Destroy(item.gameObject);//删除所有的奖励列表
        }
        foreach(var Item in currentData.rewards)
        {
            QuestUI.Instance.SetupRewardItem(Item.itemData, Item.amount);//更新奖励列表数据
        }
    }

    public void SetupNameButton(QuestData_SO questData)//更新任务列表按钮数据
    {
        currentData = questData;

        if (questData.isComplete)
            questNameText.text = questData.questName + "(Finish)";//任务完成
        else
            questNameText.text = questData.questName;//如果任务还没完成
    }
}
