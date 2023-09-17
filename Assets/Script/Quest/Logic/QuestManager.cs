using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class QuestManager : Singleton<QuestManager>
{

    [System.Serializable]
    public class QuestTask//�����б�
    {
        public QuestData_SO questData;//������Ϣ

        //��ȡ��������״̬
        public bool IsStarted { get { return questData.isStarted; } set { questData.isStarted = value; }  }

        public bool IsComplete { get { return questData.isComplete; } set { questData.isComplete = value; } }

        public bool IsFinished { get { return questData.isFinished; } set { questData.isFinished = value; } }
    }

    public List<QuestTask> tasks = new List<QuestTask>();//�����б�

    void Start()
    {
        LoadQuestManager();//��ȡ�浵
    }

    //����浵���ǰ����е�������һ�浵��Ȼ�����ȥ
    public void LoadQuestManager()//����
    {
        var questCount = PlayerPrefs.GetInt("QuestCount");//������

        for(int i = 0;i<questCount;i++)
        {
            var newQuest = ScriptableObject.CreateInstance<QuestData_SO>();//����ȫ�µ������б�

            //��ԭ�������ݰ���Ÿ��ǵ��µ������б���
            SaveManager.Instance.Load(newQuest, "task" + i);//�������
            tasks.Add(new QuestTask { questData = newQuest });//��ӵ������б���
        }
    }

    public void SaveQuestManager()//�浵
    {
        PlayerPrefs.SetInt("QuestCount", tasks.Count);//������������
        for(int i = 0;i<tasks.Count;i++)
        {
            SaveManager.Instance.Save(tasks[i].questData,"task" + i);//����˳���������
        }
    }

    public void UpdateQuestProgress(string requireName,int amount)//���������Լ�ʰȡ��Ʒ
    {
        foreach(var task in tasks)//һһ��Ӧ�����ѽ�������,����оͼ���
        {
            if (task.IsFinished)
                continue;//�������֮���������
            var matchTask = task.questData.questRequires.Find(r => r.name == requireName);//���������Ƿ���ͬ
            if (matchTask != null)
                matchTask.currentAmount += amount;//�����Ϊ�գ������������ͼ���

            task.questData.CheckQuestProgress();//��鵱ǰ�Ƿ���ɣ����Ѿ��������ԭ��Ҫ������Ƚ�
        }
    }

    public bool HaveQuest(QuestData_SO data)//�������б���������
    {
        if (data != null)
            return tasks.Any(q => q.questData.questName == data.questName);//�ж������б����Ƿ���������ͬ������
        else return false;
    }

    public QuestTask GetTask(QuestData_SO data)
    {
        return tasks.Find(q => q.questData.questName == data.questName);
    }
}
