using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    Text levelText;//等级文本

    Image healthSlider;//血条

    Image expSlider;//经验条

    void Awake()
    {
        //从child中获取子类组件
        levelText = transform.GetChild(2).GetComponent<Text>();//经验
        healthSlider = transform.GetChild(0).GetChild(0).GetComponent<Image>();//血条
        expSlider = transform.GetChild(1).GetChild(0).GetComponent<Image>();//经验条
    }

    void Update()
    {
        levelText.text = "Level" + GameManager.Instance.playerStats.characterData.currentLevel.ToString("00");
        UpdateHealth();            
        UpdateExp();
    }

    void UpdateHealth()//更新血条
    {
        float sliderPecent = (float)GameManager.Instance.playerStats.CurrentHealth / GameManager.Instance.playerStats.MaxHealth;//得出血条百分比
        healthSlider.fillAmount = sliderPecent;//赋予血条百分比
    }

    void UpdateExp()//更新经验
    {
        float sliderPecent = (float)GameManager.Instance.playerStats.characterData.currentExp/ GameManager.Instance.playerStats.characterData.baseExp;//得出经验百分比
        expSlider.fillAmount = sliderPecent;//赋予经验百分比
    }
}
