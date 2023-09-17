using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    public Text optionText;//button���ı�

    private Button thisButton;//��ť

    private DialoguePiece currentPiece;//��ťҳ��

    private bool takeQuest;//��������

    private string nextPieceID;//��һ���Ի���ID

    void Awake()
    {
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(OnOptionClicked);//���败���¼�
    }

    public void UpdateOption(DialoguePiece piece,DialogueOption option)//���¶Ի�ѡ��
    {
        //LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());//�������⣬�ڴ������Ŀ�в��ܵ��ѡ���ʱ��Ҫˢ��layout group������
        currentPiece = piece;//����Ŀ��ҳ��
        optionText.text = option.text;//����Ի�
        nextPieceID = option.targetID;//��ȡ��һ��Ŀ��Ի�
        takeQuest = option.takeQuest;//�����������״̬
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());//�������⣬�ڴ������Ŀ�в��ܵ��ѡ���ʱ��Ҫˢ��layout group������
    }

    public void OnOptionClicked()//�Ի�����¼�
    {
        if(currentPiece.quest != null)
        {
            var newTask = new QuestManager.QuestTask
            {
                questData = Instantiate(currentPiece.quest)//���������Ѿ��е������б�
            };
            if (takeQuest)//�Ѿ���������
            {
                //��ӵ������б�
                //�ж������Ƿ��Ѿ���������
                if (QuestManager.Instance.HaveQuest(newTask.questData))
                {
                    //�ж��Ƿ���ɸ��轱��
                    if(QuestManager.Instance.GetTask(newTask.questData).IsComplete)
                    {
                        //Debug.Log("�������");
                        newTask.questData.GiveRewards();//��ȡ����
                        QuestManager.Instance.GetTask(newTask.questData).IsFinished = true;//�������
                    }
                }
                else
                {
                    //û������ӵ��б���
                    QuestManager.Instance.tasks.Add(newTask);
                    QuestManager.Instance.GetTask(newTask.questData).IsStarted = true;//���´��������������״̬���true->���ı�ԭ���������ļ�

                    foreach(var requireItem in newTask.questData.RequireTargetName())
                    {
                        InventoryManager.Instance.CheckQuestItemInBag(requireItem);
                    }
                }
            }
        }

        if(nextPieceID == "")
        {
            DialogueUI.Instance.dialoguePanel.SetActive(false);//�رմ���
            return;
        }
        else
        {
            
            DialogueUI.Instance.UpdateMainDialogue(DialogueUI.Instance.currentData.dialogueIndex[nextPieceID]);//���ֵ��в�����һ��
        }
    }
}
