using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="New Dialogue",menuName = "Dialogue/Dialogue Data")]
public class DialogueData_SO : ScriptableObject
{
    public List<DialoguePiece> dialoguePieces = new List<DialoguePiece>();

    public Dictionary<string, DialoguePiece> dialogueIndex = new Dictionary<string, DialoguePiece>();//字典

#if UNITY_EDITOR
    void OnValidate()//数据被调用时就会执行
    {
        dialogueIndex.Clear();//清空字典
        foreach(var piece in dialoguePieces)
        {
            if (!dialogueIndex.ContainsKey(piece.ID))//如果新的页面id不为空
                dialogueIndex.Add(piece.ID, piece);//将id和页面数据添加上去->创建新的字典
        }
        
    }
#else
    void Awake()//保证在打包执行的游戏里第一时间获得对话的所有字典匹配 
    {
        dialogueIndex.Clear();
        foreach (var piece in dialoguePieces)
        {
            if (!dialogueIndex.ContainsKey(piece.ID))
                dialogueIndex.Add(piece.ID, piece);
        }
    }
#endif

    public QuestData_SO GetQuest()//寻找关键任务->就是说哪句话之后就接任务
    {
        QuestData_SO currentQuest = null;
        foreach(var piece in dialoguePieces)
        {
            currentQuest = piece.quest;
        }
        return currentQuest;
    }
}
