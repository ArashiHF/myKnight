using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName ="New Quest",menuName ="Quest/Quest Data")]
public class QuestData_SO : ScriptableObject
{
    [System.Serializable]
    public class QuestRequire//��������
    {
        public string name;//Ŀ������

        public int requireAmount;//Ŀ������

        public int currentAmount;//��ǰ����
    }
    public string questName;//��������

    [TextArea]
    public string description;//������

    //����׶�
    public bool isStarted;//��ʼ�׶�

    public bool isComplete;//���н׶�

    public bool isFinished;//��ɽ׶�

    public List<QuestRequire> questRequires = new List<QuestRequire>();//�����б�

    public List<InventoryItem> rewards = new List<InventoryItem>();//�����б�

    public void CheckQuestProgress()
    {
        var finishRequiress = questRequires.Where(r => r.requireAmount <= r.currentAmount);//������е�Ŀ�궼����Ŀ��ֵ

        isComplete = finishRequiress.Count() == questRequires.Count;//�Ƚ�������Ƿ����Ҫ������

        //if (isComplete)
        //    Debug.Log("�������");
    }

    public void GiveRewards()
    {
        foreach(var reward in rewards)
        {
            if(reward.amount <0)//�������Ҫ�����ռ������Ļ���Ҫ�ұ����Ϳ�����Ķ����Ƿ��㹻��Ȼ���ȥ
            {
                int requireCount = Mathf.Abs(reward.amount);//���������

                if(InventoryManager.Instance.QuestItemInBag(reward.itemData)!=null)//���ﲻΪ��
                {
                    if(InventoryManager.Instance.QuestItemInBag(reward.itemData).amount <= requireCount)
                    {
                        requireCount -= InventoryManager.Instance.QuestItemInBag(reward.itemData).amount;//��ȥ����������,ʣ���ɿ�����۳�
                        InventoryManager.Instance.QuestItemInBag(reward.itemData).amount = 0;//������������0
                        if(InventoryManager.Instance.QuestItemInAction(reward.itemData)!=null)//���������Ĳ�Ϊ�գ��Ϳۿ������
                        {
                            InventoryManager.Instance.QuestItemInAction(reward.itemData).amount -= requireCount;
                        }
                    }
                    else//������Ĵ������ֵ
                    {
                        InventoryManager.Instance.QuestItemInBag(reward.itemData).amount -= requireCount;
                    }
                }
                else//����û��ֻ�п������
                {
                    InventoryManager.Instance.QuestItemInAction(reward.itemData).amount -= requireCount;//ȫ�����ɿ�����Ŀ۵�
                }
            }
            else
            {
                InventoryManager.Instance.inventoryData.AddItem(reward.itemData, reward.amount);//ͨ���б�������Ʒ
            }

            InventoryManager.Instance.inventoryUI.RefreshUI();//����UI
            InventoryManager.Instance.actionUI.RefreshUI();//���¿����
        }
    }

    public List<string> RequireTargetName()//���ص�ǰ��������Ҫ�� �ռ�/���� ���������б�
    {
        List<string> targetNameList = new List<string>();


        foreach(var require in questRequires)//����������б�
        {
            targetNameList.Add(require.name);
        }
        return targetNameList;
    }
}
