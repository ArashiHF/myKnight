using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : Singleton<QuestUI>
{
    [Header("Elements")]
    public GameObject questPanel;//任务页面

    public ItemTooltip tooltip;//简介

    bool isOpen;//开关页面

    [Header("Quest Name")]
    public RectTransform questListTransform;//任务位置

    public QuestNameButton questNameButton;//按钮

    [Header("Text Content")]
    public Text questContentText;//任务内容文本

    [Header("Requirement")]
    public RectTransform requireTransform;//任务需求的位置

    public QuestRequirement requirement;//任务需求

    [Header("Reward Panel")]
    public RectTransform rewardTransform;//奖励的位置

    public ItemUI rewardUI;//奖励格子的ui

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            isOpen = !isOpen;//取反页面启动
            questPanel.SetActive(isOpen);//开关页面
            questContentText.text = string.Empty;//文本为空
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());//强制刷新布局
            //更新面板内容
            SetupQuestList();
            if (!isOpen)
                tooltip.gameObject.SetActive(false);//即使没有打开窗口也要关闭简介面板
        }
    }

    public void SetupQuestList()//更新任务列表
    {
        //先删除任务面板所有东西
        foreach(Transform item in questListTransform)//先删除列表中所有任务
        {
            Destroy(item.gameObject);
        }

        foreach(Transform item in rewardTransform)//删除报酬的内容
        {
            Destroy(item.gameObject);
        }

        foreach(Transform item in requireTransform)//删除需求内容
        {
            Destroy(item.gameObject);
        }

        //生成任务
        foreach(var task in QuestManager.Instance.tasks)//循环生成任务列表中的所有东西
        {
            var newTask = Instantiate(questNameButton, questListTransform);//生成列表
            newTask.SetupNameButton(task.questData);//更新任务按钮的名字
            //newTask.QuestContentText = questContentText;//将任务的内容也更新
        }
    }

    public void SetupRequireList(QuestData_SO questData)//更新任务奖励信息
    {
        questContentText.text = questData.description;//更新任务内容
        foreach (Transform item in requireTransform)//删除报酬的内容
        {
            Destroy(item.gameObject);
            //Debug.Log("删除了");
        }

        foreach(var require in questData.questRequires)
        {
            var q = Instantiate(requirement, requireTransform);//生成列表
            if (questData.isFinished)
                q.SetupRequirement(require.name,questData.isFinished);//如果任务结束则将任务变成灰色
            else
                q.SetupRequirement(require.name, require.requireAmount, require.currentAmount);//更新任务要求进度
        }
    }

    public void SetupRewardItem(ItemData_SO itemData,int amount)
    {
        var item = Instantiate(rewardUI, rewardTransform);//生成奖励列表

        item.SetupItemUI(itemData, amount);//传输数据和数量->去Namebutton用
    }
}
