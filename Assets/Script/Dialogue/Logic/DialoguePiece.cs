using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialoguePiece
{
    public string ID;//对话的代号

    public Sprite image;//对话头像

    [TextArea]
    public string text;//对话内容

    public QuestData_SO quest;//任务->是否判断有任务

    [HideInInspector]//隐藏
    public bool canExpand;//可堆叠

    public List<DialogueOption> options = new List<DialogueOption>();//对话内容
}
