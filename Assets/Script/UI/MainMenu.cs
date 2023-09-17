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

    PlayableDirector director;//������������

    void Awake()
    {
        //��ȡ��ť��� �ֱ�Ϊnewgame continue end
        newGameBtn = transform.GetChild(1).GetComponent<Button>();
        continueBtn = transform.GetChild(2).GetComponent<Button>();
        quitBtn = transform.GetChild(3).GetComponent<Button>();

        //��ӵ���¼�
        newGameBtn.onClick.AddListener(PlayTimeLine);
        continueBtn.onClick.AddListener(ContinueGame);
        quitBtn.onClick.AddListener(QuitGame);

        director = FindObjectOfType<PlayableDirector>();//���timeline����
        director.stopped += NewGame;
    }

    void PlayTimeLine()
    {
        director.Play();//��������
    }
    void NewGame(PlayableDirector obj)
    {
        //����浵Ȼ���������Ϸ
        PlayerPrefs.DeleteAll();

        SceneController.Instance.TransitionToFirstLevel();//�����һ������
    }

    void ContinueGame()
    {

        //��ȡ�浵������Ϸ
        SceneController.Instance.TransitionToLoadGame();
    }

    void QuitGame()//�ر���Ϸ
    {
        Application.Quit();
        
    }




}
