using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DialogueUI : Singleton<DialogueUI>
{
    [Header("Basic Elements")]
    public Image icon;//�Ի�ͷ��

    public Text mainText;//�Ի�����

    public Button nextButton;//next��ť

    public GameObject dialoguePanel;//�Ի�ҳ��

    [Header("Option")]
    public RectTransform optionPanel;//ѡ��λ��

    public OptionUI optionPrefab;//ѡ��Ԥ����

    [Header("Data")]
    public DialogueData_SO currentData;//��ǰ���������

    int currentIndex = 0;//�Ի�����һ����Ҫȥ��

    protected override void Awake()
    {
        base.Awake();
        nextButton.onClick.AddListener(ContinueDialogue);//��Ӻ���
    }

    void ContinueDialogue()//û��ѡ�������һ�����
    {
        if (currentIndex < currentData.dialoguePieces.Count)
            UpdateMainDialogue(currentData.dialoguePieces[currentIndex]);
        else
            dialoguePanel.SetActive(false);
    }

    public void UpdataDialogueData(DialogueData_SO data)//���¶�Ӧ��UI����
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        currentData = data;//��ȡ�Ի�����
        currentIndex = 0;
    }

    public void UpdateMainDialogue(DialoguePiece piece)
    {
        dialoguePanel.SetActive(true);//����ҳ��
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        currentIndex++;//�Ի��������
        if (piece != null)//���ҳ�治Ϊ��������ͼƬ���Ҹ�ֵ
        {
            icon.enabled = true;
            icon.sprite = piece.image;
        }
        else icon.enabled = false;//�ر�ͼ��

        mainText.text = "";//����ı�
        //mainText.text = piece.text;//���ı���ֵ
        mainText.DOText(piece.text, 1f);//��doto�����������ʾ�Ի�

        if (piece.options.Count == 0 && currentData.dialoguePieces.Count > 0)//���û��ѡ��ʹ�nextѡ��
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
            nextButton.interactable = true;
            nextButton.gameObject.SetActive(true);
            nextButton.transform.GetChild(0).gameObject.SetActive(true);//��ʾ�ı�

        }
        else
        // nextButton.gameObject.SetActive(false);//û�еĻ��͹ر�next
        {
            nextButton.interactable = false;//�رտɵ�
            nextButton.transform.GetChild(0).gameObject.SetActive(false);//�ر��ı�
        }

        //����optionѡ��
        CreateOption(piece);
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }

    void CreateOption(DialoguePiece piece)//����ѡ��,��ȫ�壬���ٴ���
    {
        if (optionPanel.childCount > 0)
        {
            for(int i = 0;i<optionPanel.childCount;i++)//�����о�ѡ�����
            {
                Destroy(optionPanel.GetChild(i).gameObject);
            }
        }
        for (int i = 0;i< piece.options.Count;i++)//������ѡ��
        {
            var option = Instantiate(optionPrefab, optionPanel);
            option.UpdateOption(piece, piece.options[i]);//ѭ��ѡ��
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }
}
