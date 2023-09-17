using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueControl : MonoBehaviour
{
    public DialogueData_SO currentData;//��ǰ�ĶԻ���Ϣ

    bool canTalk = false;//�Ƿ��ܹ��Ի�

    public GameObject Tips;
    
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")&&currentData != null)//����ڷ�Χ�ڶ����жԻ�����
        {
            canTalk = true;//�ܹ��Ի�
            //LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
            Tips.SetActive(true);//����Tips->��ҿ���
        }

    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            DialogueUI.Instance.dialoguePanel.SetActive(false);//������Ҿ͹ر�
            canTalk = false;
            Tips.SetActive(false);
        }
    }

    void Update()
    {
        if(canTalk && Input.GetMouseButtonDown(1))//�ڷ�Χ�ڶ��ҵ���Ҽ�
        {
            OpenDialogue();//��UI
            Tips.SetActive(false);
        }
    }

    void OpenDialogue()
    {
        //Debug.Log("��UI");
        //��UI���
        DialogueUI.Instance.UpdataDialogueData(currentData);//����ui���ҳ�ʼ��
        //���佻������
        DialogueUI.Instance.UpdateMainDialogue(currentData.dialoguePieces[0]);//��������
    }
}
