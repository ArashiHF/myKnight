using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : Singleton<SaveManager>
{
    string sceneName = "";//��������

    public string SceneName { get { return PlayerPrefs.GetString(sceneName); } }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SavePlayerData();
            SceneController.Instance.TranitionToMain();//返回主菜单
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            SavePlayerData();
        }
        if(Input.GetKeyDown(KeyCode.L))
        {
            LoadPlayerData();
        }
    }

    public void SavePlayerData()
    {
        Save(GameManager.Instance.playerStats.characterData, GameManager.Instance.playerStats.characterData.name);//�洢������ֺ͵�ǰ����
    }
    public void LoadPlayerData()
    {
        Load(GameManager.Instance.playerStats.characterData, GameManager.Instance.playerStats.characterData.name);//��ȡ����
    }

    public void Save(object data,string key)
    {
        var jsonData = JsonUtility.ToJson(data,true);//ת������Ϊjson����
        PlayerPrefs.SetString(key, jsonData);//��������ݱ���

        PlayerPrefs.SetString(sceneName, SceneManager.GetActiveScene().name);//�洢��������
        PlayerPrefs.Save();
    }

    public void Load(Object data,string key)
    {
        if(PlayerPrefs.HasKey(key))//如果存在存档则直接给存档
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), data);
        }
    }
}
