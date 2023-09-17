using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    Text levelText;//�ȼ��ı�

    Image healthSlider;//Ѫ��

    Image expSlider;//������

    void Awake()
    {
        //��child�л�ȡ�������
        levelText = transform.GetChild(2).GetComponent<Text>();//����
        healthSlider = transform.GetChild(0).GetChild(0).GetComponent<Image>();//Ѫ��
        expSlider = transform.GetChild(1).GetChild(0).GetComponent<Image>();//������
    }

    void Update()
    {
        levelText.text = "Level" + GameManager.Instance.playerStats.characterData.currentLevel.ToString("00");
        UpdateHealth();            
        UpdateExp();
    }

    void UpdateHealth()//����Ѫ��
    {
        float sliderPecent = (float)GameManager.Instance.playerStats.CurrentHealth / GameManager.Instance.playerStats.MaxHealth;//�ó�Ѫ���ٷֱ�
        healthSlider.fillAmount = sliderPecent;//����Ѫ���ٷֱ�
    }

    void UpdateExp()//���¾���
    {
        float sliderPecent = (float)GameManager.Instance.playerStats.characterData.currentExp/ GameManager.Instance.playerStats.characterData.baseExp;//�ó�����ٷֱ�
        expSlider.fillAmount = sliderPecent;//���辭��ٷֱ�
    }
}
