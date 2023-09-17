using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditorInternal;
using System.IO;

[CustomEditor(typeof(DialogueData_SO))]//当前脚本影响的数据
public class DialogueCustomEditor : Editor
{
    //画按钮
    public override void OnInspectorGUI()//绘制UI
    {
        //绘制布局
        if(GUILayout.Button("Open in Editor"))//在Dialogue数据中增加一个按钮
        {
            DialogueEditor.InitWindow((DialogueData_SO)target);//创建initwindow类型数据,强制类型转化类型
        }
        base.OnInspectorGUI();
        
    }
}
public class DialogueEditor : EditorWindow
{//插件编辑器,打包的时候不会出现
    DialogueData_SO currentData;//当前的任务文本数据

    ReorderableList piecesList = null;//对话列表->用于编辑对话

    Vector2 scrollPos = Vector2.zero;

    Dictionary<string,ReorderableList> optionListDict = new Dictionary<string,ReorderableList>();//字典列表->名字和id

    [MenuItem("Arashi/Dialogue Editor")]//命名空间
    public static void Init()
    {
        DialogueEditor editorWindow = GetWindow<DialogueEditor>("Dialogue Editor");//生成窗口,名为Dialogue Editor
        editorWindow.autoRepaintOnSceneChange = true;//实时更新插件数据
    }
    
    public static void InitWindow(DialogueData_SO data)
    {
        DialogueEditor editorWindow = GetWindow<DialogueEditor>("Dialogue Editor");//打开窗口
        editorWindow.currentData= data;//打开的窗口获取当前的任务数据
    }

    [OnOpenAsset]
    public static bool OnOpenAsset(int instanceID,int ID)//如果从别的地方打开窗口也是会打开这个创建的窗口
    {
        DialogueData_SO data = EditorUtility.InstanceIDToObject(instanceID) as DialogueData_SO;
        if(data!=null)
        {
            DialogueEditor.InitWindow(data);
            return true;
        }
        return false;
    }

    void OnSelectionChange()
    {
        //选择改变时调用一次
        //先创造一个变量用来存储原本变量，然后再打开别的变量之后相互比较
        var newData = Selection.activeObject as DialogueData_SO;//当前激活对象数据

        if(newData != null)
        {
            //刷新列表
            currentData = newData;
            SetupReorderableList();
        }
        else
        {
            //清空列表
            currentData = null;
            piecesList = null;
        }
        Repaint();//重新绘制界面
    }
    void OnGUI()//显示面板数据
    {
        //数据
        if(currentData !=null)//如果数据非空则能够进行绘制
        {
            EditorGUILayout.LabelField(currentData.name,EditorStyles.boldLabel);//有数据输出名字并且粗体
            GUILayout.Space(10);
            
            scrollPos = GUILayout.BeginScrollView(scrollPos,GUILayout.ExpandWidth(true),GUILayout.ExpandHeight(true));//每次显示面板的时候都会更新滑动条
            if(piecesList == null)//对话列表
                SetupReorderableList();//如果没有列表数据则更新
            piecesList.DoLayoutList();//生成列表数据->还能添加
            GUILayout.EndScrollView();//结束滑动条
        }
        else
        {
            //如果为空的话添加按钮查找文件
            if(GUILayout.Button("Create New Dialogue"))
            {
                string dataPath =  "Assets/Game Data/Dialogue Data/";//文件夹名字 最后有个 / 表示文件夹

                if(Directory.Exists(dataPath))//如果有文件但没有在字典里面就在字典中创建
                    Directory.CreateDirectory(dataPath);

                DialogueData_SO newData = ScriptableObject.CreateInstance<DialogueData_SO>();//创建全新数据！！！不是文件

                AssetDatabase.CreateAsset(newData,dataPath + "/" + "New Dialogue.asset");//创建新文件
                currentData = newData;//赋予数据
            }
            GUILayout.Label("NO DATA SELETED",EditorStyles.boldLabel);//没有则输出语句并且为粗体
        }
    }

    void OnDisable()
    {
        optionListDict.Clear();//清空列表
    }

    private void SetupReorderableList()
    {
        piecesList = new ReorderableList(currentData.dialoguePieces,typeof(DialoguePiece),true,true,true,true);//生成对话列表

        piecesList.drawHeaderCallback += OnDrawPieceHeader;//绘制标题栏

        piecesList.drawElementCallback += OnDrawPieceListElement;//绘制内容页

        piecesList.elementHeightCallback += OnHeightChanged;//需要修改照片大小
    }

    private float OnHeightChanged(int index)//如果高度不对则要修改
    {
        return GetPieceHeight(currentData.dialoguePieces[index]);//返回需要修改的高度大小
    }

    float GetPieceHeight(DialoguePiece piece)
    {
        var height = EditorGUIUtility.singleLineHeight;

        var isExpand = piece.canExpand;

        if(isExpand)
        {
            height += EditorGUIUtility.singleLineHeight * 9;//一共九行

            var options = piece.options;//多一个就加一行
            {
                height += EditorGUIUtility.singleLineHeight * options.Count;
            }
        }
        return height;
    }

    private void OnDrawPieceListElement(Rect rect,int index,bool isActive,bool isFocused)
    {
        EditorUtility.SetDirty(currentData);//让数据变得可以撤销保存等操作。

        GUIStyle textStyle = new GUIStyle("TextField");
        if(index < currentData.dialoguePieces.Count)//表格的数据必须小于对话文件的数据
        {
            var currentPiece = currentData.dialoguePieces[index];//循环获取每个data的页面数据

            var tempRect = rect;//复制的表格数据

            tempRect.height = EditorGUIUtility.singleLineHeight;//设定高度

            currentPiece.canExpand = EditorGUI.Foldout(tempRect,currentPiece.canExpand,currentPiece.ID);
            if(currentPiece.canExpand)
            {
            //显示ID
            tempRect.width = 30;//宽度30
            tempRect.y += tempRect.height;//添加高度
            EditorGUI.LabelField(tempRect,"ID");//显示ID并且能够修改

            //在ID右边显示正确的ID数据
            tempRect.x += tempRect.width;//右移一段然后生成其他内容
            tempRect.width = 100;
            currentPiece.ID = EditorGUI.TextField(tempRect,currentPiece.ID);

            //显示Quest
            tempRect.x += tempRect.width + 10;//右移10位
            EditorGUI.LabelField(tempRect,"Quest");//显示Quest

            //显示Quest文件
            tempRect.x += 50;
            currentPiece.quest = (QuestData_SO)EditorGUI.ObjectField(tempRect,currentPiece.quest,typeof(QuestData_SO),false);//显示Quest文件并且不可修改

            //移动到下一行
            tempRect.y += EditorGUIUtility.singleLineHeight + 5;//下移五位
            tempRect.x = rect.x;//回归左边

            //添加图片
            tempRect.height = 60;
            tempRect.width = tempRect.height;//长宽都为60
            currentPiece.image = (Sprite)EditorGUI.ObjectField(tempRect,currentPiece.image,typeof(Sprite),false);//添加图片内容

            //text文本框部分
            tempRect.x += tempRect.width + 5;
            tempRect.width = rect.width - tempRect.x;
            textStyle.wordWrap = true;
            currentPiece.text = (string)EditorGUI.TextField(tempRect, currentPiece.text,textStyle);//绘制文本框并且能够自动换行

            //选项页->初始化
            tempRect.y += tempRect.height + 5;
            tempRect.x = rect.x;
            tempRect.width = rect.width;

            //字典部分
            string optionListKey = currentPiece.ID + currentPiece.text;//字典又ID+文本页面内容组成

            if(optionListKey != string.Empty)
            {
                if(!optionListDict.ContainsKey(optionListKey))
                {
                    var optionlist = new ReorderableList(currentPiece.options,typeof(DialogueOption),true,true,true,true);

                    optionlist.drawHeaderCallback = OnDrawOptionHeader;//选项列表头

                    optionlist.drawElementCallback = (optionRect,optionIndex,optionActive,optionFocused) =>
                    {
                        OnDrawOptionElement(currentPiece,optionRect,optionIndex,optionActive,optionFocused);
                    };
                    optionListDict[optionListKey] = optionlist;//赋值选项
                }

                optionListDict[optionListKey].DoList(tempRect);//创造列表
            }
            }              
        }
    }

    private void OnDrawOptionHeader(Rect rect)//编辑选项列表头
    {
        GUI.Label(rect,"Option Text");
        rect.x += rect.width * 0.5f + 10;
        GUI.Label(rect,"Target ID");
        rect.x += rect.width * 0.3f;
        GUI.Label(rect,"Apply");
    }

    private void OnDrawOptionElement(DialoguePiece currentPiece, Rect optionRect,int optionIndex, bool optionActive, bool optionFocused)
    {
        
        var currentOption = currentPiece.options[optionIndex];
        var tempRect = optionRect;
        
        //选项内容
        tempRect.width = optionRect.width * 0.5f;
        currentOption.text = EditorGUI.TextField(tempRect,currentOption.text);

        //选项目标
        tempRect.x += tempRect.width + 5;
        tempRect.width = optionRect.width * 0.3f;
        currentOption.targetID = EditorGUI.TextField(tempRect,currentOption.targetID);

        //接受任务框
        tempRect.x += tempRect.width + 5;
        tempRect.width = optionRect.width * 0.2f;
        currentOption.takeQuest = EditorGUI.Toggle(tempRect,currentOption.takeQuest);
    }

    private void OnDrawPieceHeader(Rect rect)
    {
        GUI.Label(rect,"Dialogue Pieces");//绘制框框->能用来做标题或者表格行
    }

    
}

