using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    public Text optionText;//button的文本

    private Button thisButton;//按钮

    private DialoguePiece currentPiece;//按钮页面

    private bool takeQuest;//接受任务

    private string nextPieceID;//下一个对话的ID

    void Awake()
    {
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(OnOptionClicked);//赋予触发事件
    }

    public void UpdateOption(DialoguePiece piece,DialogueOption option)//更新对话选项
    {
        //LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());//核心问题，在打包的项目中不能点击选项，此时需要刷新layout group的内容
        currentPiece = piece;//赋予目标页面
        optionText.text = option.text;//赋予对话
        nextPieceID = option.targetID;//获取下一个目标对话
        takeQuest = option.takeQuest;//变更接受任务状态
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());//核心问题，在打包的项目中不能点击选项，此时需要刷新layout group的内容
    }

    public void OnOptionClicked()//对话点击事件
    {
        if(currentPiece.quest != null)
        {
            var newTask = new QuestManager.QuestTask
            {
                questData = Instantiate(currentPiece.quest)//传输现在已经有的人物列表
            };
            if (takeQuest)//已经接受任务
            {
                //添加到任务列表
                //判断现在是否已经接受任务
                if (QuestManager.Instance.HaveQuest(newTask.questData))
                {
                    //判断是否完成给予奖励
                    if(QuestManager.Instance.GetTask(newTask.questData).IsComplete)
                    {
                        //Debug.Log("任务完成");
                        newTask.questData.GiveRewards();//领取奖励
                        QuestManager.Instance.GetTask(newTask.questData).IsFinished = true;//完成任务
                    }
                }
                else
                {
                    //没任务，添加到列表中
                    QuestManager.Instance.tasks.Add(newTask);
                    QuestManager.Instance.GetTask(newTask.questData).IsStarted = true;//将新创建的任务的任务状态变成true->不改变原本的任务文件

                    foreach(var requireItem in newTask.questData.RequireTargetName())
                    {
                        InventoryManager.Instance.CheckQuestItemInBag(requireItem);
                    }
                }
            }
        }

        if(nextPieceID == "")
        {
            DialogueUI.Instance.dialoguePanel.SetActive(false);//关闭窗口
            return;
        }
        else
        {
            
            DialogueUI.Instance.UpdateMainDialogue(DialogueUI.Instance.currentData.dialogueIndex[nextPieceID]);//在字典中查找下一个
        }
    }
}
