using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialoguePiece
{
    public string ID;//�Ի��Ĵ���

    public Sprite image;//�Ի�ͷ��

    [TextArea]
    public string text;//�Ի�����

    public QuestData_SO quest;//����->�Ƿ��ж�������

    [HideInInspector]//����
    public bool canExpand;//�ɶѵ�

    public List<DialogueOption> options = new List<DialogueOption>();//�Ի�����
}
