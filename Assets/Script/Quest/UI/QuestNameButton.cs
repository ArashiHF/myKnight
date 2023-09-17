using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestNameButton : MonoBehaviour
{
    public Text questNameText;//����ť����

    public QuestData_SO currentData;//��ǰ����

    public Text QuestContentText;//�����ı�

    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(UpdateQuestContent);//��ӵ��֮�������������
    }

    void UpdateQuestContent()//�����������ݵ�������Ϣ
    {
        //QuestContentText.text = currentData.description;//�����ı�����
        QuestUI.Instance.SetupRequireList(currentData);//��������Ҫ�������

        foreach(Transform item in QuestUI.Instance.rewardTransform)
        {
            Destroy(item.gameObject);//ɾ�����еĽ����б�
        }
        foreach(var Item in currentData.rewards)
        {
            QuestUI.Instance.SetupRewardItem(Item.itemData, Item.amount);//���½����б�����
        }
    }

    public void SetupNameButton(QuestData_SO questData)//���������б�ť����
    {
        currentData = questData;

        if (questData.isComplete)
            questNameText.text = questData.questName + "(Finish)";//�������
        else
            questNameText.text = questData.questName;//�������û���
    }
}
