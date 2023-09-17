using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class QuestManager : Singleton<QuestManager>
{

    [System.Serializable]
    public class QuestTask//任务列表
    {
        public QuestData_SO questData;//任务信息

        //获取现在任务状态
        public bool IsStarted { get { return questData.isStarted; } set { questData.isStarted = value; }  }

        public bool IsComplete { get { return questData.isComplete; } set { questData.isComplete = value; } }

        public bool IsFinished { get { return questData.isFinished; } set { questData.isFinished = value; } }
    }

    public List<QuestTask> tasks = new List<QuestTask>();//任务列表

    void Start()
    {
        LoadQuestManager();//读取存档
    }

    //任务存档就是把所有的任务逐一存档，然后加上去
    public void LoadQuestManager()//载入
    {
        var questCount = PlayerPrefs.GetInt("QuestCount");//任务数

        for(int i = 0;i<questCount;i++)
        {
            var newQuest = ScriptableObject.CreateInstance<QuestData_SO>();//生成全新的任务列表

            //将原本的数据按序号覆盖到新的任务列表中
            SaveManager.Instance.Load(newQuest, "task" + i);//根据序号
            tasks.Add(new QuestTask { questData = newQuest });//添加到任务列表中
        }
    }

    public void SaveQuestManager()//存档
    {
        PlayerPrefs.SetInt("QuestCount", tasks.Count);//保存任务数量
        for(int i = 0;i<tasks.Count;i++)
        {
            SaveManager.Instance.Save(tasks[i].questData,"task" + i);//根据顺序存下数据
        }
    }

    public void UpdateQuestProgress(string requireName,int amount)//敌人死亡以及拾取物品
    {
        foreach(var task in tasks)//一一对应所有已接受任务,如果有就加上
        {
            if (task.IsFinished)
                continue;//完成任务之后就跳过了
            var matchTask = task.questData.questRequires.Find(r => r.name == requireName);//查找名字是否相同
            if (matchTask != null)
                matchTask.currentAmount += amount;//如果不为空，而且有数量就加上

            task.questData.CheckQuestProgress();//检查当前是否完成（用已经完成数与原本要求数相比较
        }
    }

    public bool HaveQuest(QuestData_SO data)//在任务列表中有任务
    {
        if (data != null)
            return tasks.Any(q => q.questData.questName == data.questName);//判断任务列表中是否有名字相同的任务
        else return false;
    }

    public QuestTask GetTask(QuestData_SO data)
    {
        return tasks.Find(q => q.questData.questName == data.questName);
    }
}
