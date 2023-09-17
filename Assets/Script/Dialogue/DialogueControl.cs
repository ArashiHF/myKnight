using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueControl : MonoBehaviour
{
    public DialogueData_SO currentData;//当前的对话信息

    bool canTalk = false;//是否能够对话

    public GameObject Tips;
    
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")&&currentData != null)//玩家在范围内而且有对话内容
        {
            canTalk = true;//能够对话
            //LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
            Tips.SetActive(true);//开启Tips->玩家靠近
        }

    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            DialogueUI.Instance.dialoguePanel.SetActive(false);//脱离玩家就关闭
            canTalk = false;
            Tips.SetActive(false);
        }
    }

    void Update()
    {
        if(canTalk && Input.GetMouseButtonDown(1))//在范围内而且点击右键
        {
            OpenDialogue();//打开UI
            Tips.SetActive(false);
        }
    }

    void OpenDialogue()
    {
        //Debug.Log("打开UI");
        //打开UI面板
        DialogueUI.Instance.UpdataDialogueData(currentData);//启动ui并且初始化
        //传输交流数据
        DialogueUI.Instance.UpdateMainDialogue(currentData.dialoguePieces[0]);//传输数据
    }
}
