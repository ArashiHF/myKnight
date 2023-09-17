using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="New Dialogue",menuName = "Dialogue/Dialogue Data")]
public class DialogueData_SO : ScriptableObject
{
    public List<DialoguePiece> dialoguePieces = new List<DialoguePiece>();

    public Dictionary<string, DialoguePiece> dialogueIndex = new Dictionary<string, DialoguePiece>();//�ֵ�

#if UNITY_EDITOR
    void OnValidate()//���ݱ�����ʱ�ͻ�ִ��
    {
        dialogueIndex.Clear();//����ֵ�
        foreach(var piece in dialoguePieces)
        {
            if (!dialogueIndex.ContainsKey(piece.ID))//����µ�ҳ��id��Ϊ��
                dialogueIndex.Add(piece.ID, piece);//��id��ҳ�����������ȥ->�����µ��ֵ�
        }
        
    }
#else
    void Awake()//��֤�ڴ��ִ�е���Ϸ���һʱ���öԻ��������ֵ�ƥ�� 
    {
        dialogueIndex.Clear();
        foreach (var piece in dialoguePieces)
        {
            if (!dialogueIndex.ContainsKey(piece.ID))
                dialogueIndex.Add(piece.ID, piece);
        }
    }
#endif

    public QuestData_SO GetQuest()//Ѱ�ҹؼ�����->����˵�ľ仰֮��ͽ�����
    {
        QuestData_SO currentQuest = null;
        foreach(var piece in dialoguePieces)
        {
            currentQuest = piece.quest;
        }
        return currentQuest;
    }
}
