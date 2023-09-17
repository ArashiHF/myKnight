using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName ="New Quest",menuName ="Quest/Quest Data")]
public class QuestData_SO : ScriptableObject
{
    [System.Serializable]
    public class QuestRequire//任务需求
    {
        public string name;//目标名字

        public int requireAmount;//目标数量

        public int currentAmount;//当前数量
    }
    public string questName;//任务名字

    [TextArea]
    public string description;//任务简介

    //任务阶段
    public bool isStarted;//开始阶段

    public bool isComplete;//进行阶段

    public bool isFinished;//完成阶段

    public List<QuestRequire> questRequires = new List<QuestRequire>();//任务列表

    public List<InventoryItem> rewards = new List<InventoryItem>();//奖励列表

    public void CheckQuestProgress()
    {
        var finishRequiress = questRequires.Where(r => r.requireAmount <= r.currentAmount);//如果所有的目标都等于目标值

        isComplete = finishRequiress.Count() == questRequires.Count;//比较完成数是否等于要求数量

        //if (isComplete)
        //    Debug.Log("任务完成");
    }

    public void GiveRewards()
    {
        foreach(var reward in rewards)
        {
            if(reward.amount <0)//如果任务要求是收集东西的话就要找背包和快捷栏的东西是否足够，然后扣去
            {
                int requireCount = Mathf.Abs(reward.amount);//需求的数量

                if(InventoryManager.Instance.QuestItemInBag(reward.itemData)!=null)//包里不为空
                {
                    if(InventoryManager.Instance.QuestItemInBag(reward.itemData).amount <= requireCount)
                    {
                        requireCount -= InventoryManager.Instance.QuestItemInBag(reward.itemData).amount;//减去背包的数量,剩下由快捷栏扣除
                        InventoryManager.Instance.QuestItemInBag(reward.itemData).amount = 0;//背包的数量清0
                        if(InventoryManager.Instance.QuestItemInAction(reward.itemData)!=null)//如果快捷栏的不为空，就扣快捷栏的
                        {
                            InventoryManager.Instance.QuestItemInAction(reward.itemData).amount -= requireCount;
                        }
                    }
                    else//包里面的大于这个值
                    {
                        InventoryManager.Instance.QuestItemInBag(reward.itemData).amount -= requireCount;
                    }
                }
                else//包里没有只有快捷栏有
                {
                    InventoryManager.Instance.QuestItemInAction(reward.itemData).amount -= requireCount;//全部都由快捷栏的扣掉
                }
            }
            else
            {
                InventoryManager.Instance.inventoryData.AddItem(reward.itemData, reward.amount);//通过列表增加物品
            }

            InventoryManager.Instance.inventoryUI.RefreshUI();//更新UI
            InventoryManager.Instance.actionUI.RefreshUI();//更新快捷栏
        }
    }

    public List<string> RequireTargetName()//返回当前任务所需要的 收集/消灭 任务需求列表
    {
        List<string> targetNameList = new List<string>();


        foreach(var require in questRequires)//将需求加入列表
        {
            targetNameList.Add(require.name);
        }
        return targetNameList;
    }
}
