using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DialogueControl))]
public class QuestGiver : MonoBehaviour
{
    DialogueControl controller;//�Ի�������

    QuestData_SO currentQuest;//����

    public DialogueData_SO startDialogue;//����ʼʱ�Ļ�

    public DialogueData_SO progressDialogue;//������й����еĶԻ�

    public DialogueData_SO completeDialogue;//�������ʱ���������˵�Ļ�

    public DialogueData_SO finishDialogue;//��������֮����˵�Ļ�

    #region ȷ���ֽ׶�����״̬
    public bool IsStarted
    { 
        get
        {
            if (QuestManager.Instance.HaveQuest(currentQuest))//�жϵ�ǰ�Ƿ�������
            {
                return QuestManager.Instance.GetTask(currentQuest).IsStarted;//�������״̬��Ϊfalse
            }
            else
                return false;//����Ϊû������
        }
    }

    public bool IsComplete
    {
        get
        {
            if (QuestManager.Instance.HaveQuest(currentQuest))//�жϵ�ǰ�Ƿ�������
            {
                return QuestManager.Instance.GetTask(currentQuest).IsComplete;//�������״̬��Ϊfalse
            }
            else
                return false;//����Ϊû������
        }
    }

    public bool IsFinished
    {
        get
        {
            if (QuestManager.Instance.HaveQuest(currentQuest))//�жϵ�ǰ�Ƿ�������
            {
                return QuestManager.Instance.GetTask(currentQuest).IsFinished;//�������״̬��Ϊfalse
            }
            else
                return false;//����Ϊû������
        }
    }
    #endregion

    void Awake()
    {
        controller = GetComponent<DialogueControl>();
    }
    
    void Start()
    {
        controller.currentData = startDialogue;//��״̬��ɳ�ʼ״̬

        currentQuest = controller.currentData.GetQuest();//ȷʵ�Ի����Ƿ�������
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
