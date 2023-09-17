using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DialogueUI : Singleton<DialogueUI>
{
    [Header("Basic Elements")]
    public Image icon;//对话头像

    public Text mainText;//对话内容

    public Button nextButton;//next按钮

    public GameObject dialoguePanel;//对话页面

    [Header("Option")]
    public RectTransform optionPanel;//选项位置

    public OptionUI optionPrefab;//选项预制体

    [Header("Data")]
    public DialogueData_SO currentData;//当前聊天的数据

    int currentIndex = 0;//对话的下一步是要去哪

    protected override void Awake()
    {
        base.Awake();
        nextButton.onClick.AddListener(ContinueDialogue);//添加函数
    }

    void ContinueDialogue()//没有选项，加载下一条语句
    {
        if (currentIndex < currentData.dialoguePieces.Count)
            UpdateMainDialogue(currentData.dialoguePieces[currentIndex]);
        else
            dialoguePanel.SetActive(false);
    }

    public void UpdataDialogueData(DialogueData_SO data)//更新对应的UI数据
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        currentData = data;//获取对话数据
        currentIndex = 0;
    }

    public void UpdateMainDialogue(DialoguePiece piece)
    {
        dialoguePanel.SetActive(true);//启动页面
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        currentIndex++;//对话序号增加
        if (piece != null)//如果页面不为空则启动图片而且赋值
        {
            icon.enabled = true;
            icon.sprite = piece.image;
        }
        else icon.enabled = false;//关闭图像

        mainText.text = "";//清空文本
        //mainText.text = piece.text;//给文本赋值
        mainText.DOText(piece.text, 1f);//用doto插件，打字显示对话

        if (piece.options.Count == 0 && currentData.dialoguePieces.Count > 0)//如果没有选项就打开next选项
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
            nextButton.interactable = true;
            nextButton.gameObject.SetActive(true);
            nextButton.transform.GetChild(0).gameObject.SetActive(true);//显示文本

        }
        else
        // nextButton.gameObject.SetActive(false);//没有的话就关闭next
        {
            nextButton.interactable = false;//关闭可点
            nextButton.transform.GetChild(0).gameObject.SetActive(false);//关闭文本
        }

        //创建option选项
        CreateOption(piece);
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }

    void CreateOption(DialoguePiece piece)//创建选项,先全清，后再创建
    {
        if (optionPanel.childCount > 0)
        {
            for(int i = 0;i<optionPanel.childCount;i++)//把所有旧选项清除
            {
                Destroy(optionPanel.GetChild(i).gameObject);
            }
        }
        for (int i = 0;i< piece.options.Count;i++)//生成新选项
        {
            var option = Instantiate(optionPrefab, optionPanel);
            option.UpdateOption(piece, piece.options[i]);//循环选项
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }
}
