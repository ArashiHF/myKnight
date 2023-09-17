using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class MainMenu : MonoBehaviour
{
    Button newGameBtn;

    Button continueBtn;

    Button quitBtn;

    PlayableDirector director;//开场动画控制

    void Awake()
    {
        //获取按钮组件 分别为newgame continue end
        newGameBtn = transform.GetChild(1).GetComponent<Button>();
        continueBtn = transform.GetChild(2).GetComponent<Button>();
        quitBtn = transform.GetChild(3).GetComponent<Button>();

        //添加点击事件
        newGameBtn.onClick.AddListener(PlayTimeLine);
        continueBtn.onClick.AddListener(ContinueGame);
        quitBtn.onClick.AddListener(QuitGame);

        director = FindObjectOfType<PlayableDirector>();//获得timeline动画
        director.stopped += NewGame;
    }

    void PlayTimeLine()
    {
        director.Play();//启动动画
    }
    void NewGame(PlayableDirector obj)
    {
        //清除存档然后进入新游戏
        PlayerPrefs.DeleteAll();

        SceneController.Instance.TransitionToFirstLevel();//进入第一个场景
    }

    void ContinueGame()
    {

        //读取存档进入游戏
        SceneController.Instance.TransitionToLoadGame();
    }

    void QuitGame()//关闭游戏
    {
        Application.Quit();
        
    }




}
