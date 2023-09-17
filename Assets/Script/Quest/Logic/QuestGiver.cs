using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DialogueControl))]
public class QuestGiver : MonoBehaviour
{
    DialogueControl controller;//对话控制器

    QuestData_SO currentQuest;//任务

    public DialogueData_SO startDialogue;//任务开始时的话

    public DialogueData_SO progressDialogue;//任务进行过程中的对话

    public DialogueData_SO completeDialogue;//完成任务时候给奖励所说的话

    public DialogueData_SO finishDialogue;//结束任务之后所说的话

    #region 确认现阶段任务状态
    public bool IsStarted
    { 
        get
        {
            if (QuestManager.Instance.HaveQuest(currentQuest))//判断当前是否有任务
            {
                return QuestManager.Instance.GetTask(currentQuest).IsStarted;//变更任务状态变为false
            }
            else
                return false;//否则将为没有任务
        }
    }

    public bool IsComplete
    {
        get
        {
            if (QuestManager.Instance.HaveQuest(currentQuest))//判断当前是否有任务
            {
                return QuestManager.Instance.GetTask(currentQuest).IsComplete;//变更任务状态变为false
            }
            else
                return false;//否则将为没有任务
        }
    }

    public bool IsFinished
    {
        get
        {
            if (QuestManager.Instance.HaveQuest(currentQuest))//判断当前是否有任务
            {
                return QuestManager.Instance.GetTask(currentQuest).IsFinished;//变更任务状态变为false
            }
            else
                return false;//否则将为没有任务
        }
    }
    #endregion

    void Awake()
    {
        controller = GetComponent<DialogueControl>();
    }
    
    void Start()
    {
        controller.currentData = startDialogue;//将状态变成初始状态

        currentQuest = controller.currentData.GetQuest();//确实对话中是否有任务
    }

    void Update()
    {
        if(IsStarted)
        {
            if(IsComplete)
            {
                controller.currentData = completeDialogue;
            }
            else
            {
                controller.currentData = progressDialogue;
            }
        }

        if(IsFinished)
        {
            controller.currentData = finishDialogue;
        }
    }
}
