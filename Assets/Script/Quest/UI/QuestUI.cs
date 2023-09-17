using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : Singleton<QuestUI>
{
    [Header("Elements")]
    public GameObject questPanel;//����ҳ��

    public ItemTooltip tooltip;//���

    bool isOpen;//����ҳ��

    [Header("Quest Name")]
    public RectTransform questListTransform;//����λ��

    public QuestNameButton questNameButton;//��ť

    [Header("Text Content")]
    public Text questContentText;//���������ı�

    [Header("Requirement")]
    public RectTransform requireTransform;//���������λ��

    public QuestRequirement requirement;//��������

    [Header("Reward Panel")]
    public RectTransform rewardTransform;//������λ��

    public ItemUI rewardUI;//�������ӵ�ui

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            isOpen = !isOpen;//ȡ��ҳ������
            questPanel.SetActive(isOpen);//����ҳ��
            questContentText.text = string.Empty;//�ı�Ϊ��
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());//ǿ��ˢ�²���
            //�����������
            SetupQuestList();
            if (!isOpen)
                tooltip.gameObject.SetActive(false);//��ʹû�д򿪴���ҲҪ�رռ�����
        }
    }

    public void SetupQuestList()//���������б�
    {
        //��ɾ������������ж���
        foreach(Transform item in questListTransform)//��ɾ���б�����������
        {
            Destroy(item.gameObject);
        }

        foreach(Transform item in rewardTransform)//ɾ�����������
        {
            Destroy(item.gameObject);
        }

        foreach(Transform item in requireTransform)//ɾ����������
        {
            Destroy(item.gameObject);
        }

        //��������
        foreach(var task in QuestManager.Instance.tasks)//ѭ�����������б��е����ж���
        {
            var newTask = Instantiate(questNameButton, questListTransform);//�����б�
            newTask.SetupNameButton(task.questData);//��������ť������
            //newTask.QuestContentText = questContentText;//�����������Ҳ����
        }
    }

    public void SetupRequireList(QuestData_SO questData)//������������Ϣ
    {
        questContentText.text = questData.description;//������������
        foreach (Transform item in requireTransform)//ɾ�����������
        {
            Destroy(item.gameObject);
            //Debug.Log("ɾ����");
        }

        foreach(var require in questData.questRequires)
        {
            var q = Instantiate(requirement, requireTransform);//�����б�
            if (questData.isFinished)
                q.SetupRequirement(require.name,questData.isFinished);//�����������������ɻ�ɫ
            else
                q.SetupRequirement(require.name, require.requireAmount, require.currentAmount);//��������Ҫ�����
        }
    }

    public void SetupRewardItem(ItemData_SO itemData,int amount)
    {
        var item = Instantiate(rewardUI, rewardTransform);//���ɽ����б�

        item.SetupItemUI(itemData, amount);//�������ݺ�����->ȥNamebutton��
    }
}
