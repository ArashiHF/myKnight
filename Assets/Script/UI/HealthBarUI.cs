using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public GameObject healthUIPrefab;//Ѫ��Ԥ����

    public Transform barPoint;//Ѫ��λ��

    public bool alwayVisible;//���ÿɼ���������Ƴ��Ƿ񳤾ÿɼ�

    public float visibleTime;//�ɼ�ʱ��

    private float timeLeft;//ʣ����ʾʱ��

    Image healthSlider;//Ѫ��

    Transform UIbar;//UI����

    Transform cam;//���λ�ã���Ѫ����׼���

    CharacterStats currentStats;//��ǰ״̬

    void Awake()
    {
        currentStats = GetComponent<CharacterStats>();//��õ�ǰ����

        currentStats.UpdataHealthBarOnAttack += UpdateHealthBar; //��Ӹ���Ѫ������
    }

    void OnEnable()
    {
        cam = Camera.main.transform;//��ȡ���λ��

        foreach(Canvas canvas in FindObjectsOfType<Canvas>())//��������canvas
        {
            if(canvas.renderMode == RenderMode.WorldSpace)//���ΪworldSpace
            {
                UIbar = Instantiate(healthUIPrefab, canvas.transform).transform;//��ȡ��ǰ�������λ��

                healthSlider = UIbar.GetChild(0).GetComponent<Image>();//���Ѫ��

                UIbar.gameObject.SetActive(alwayVisible);//ƽ��ȷ���Ƿ�ɼ�
            }
        }
    }

    private void UpdateHealthBar(int currentHealth, int maxHealth)//����Ѫ��
    {
        if (currentHealth <= 0)
            Destroy(UIbar.gameObject);//ɾ��Ѫ��
        UIbar.gameObject.SetActive(true);//�����˾Ϳɼ�
        timeLeft = visibleTime;
        float sliderPercent = (float)currentHealth / maxHealth;//Ѫ���ٷֱ���
        healthSlider.fillAmount = sliderPercent;//����Ѫ��
    }

    void LateUpdate()
    {
        if(UIbar != null)
        {
            UIbar.position = barPoint.position;//����Ѫ������
            UIbar.forward = -cam.forward;//Ѫ��ʱ�̳��������������ķ�����ΪѪ������

            if (timeLeft <= 0 && !alwayVisible)//�������ʣ��ʱ�䲢�Ҳ�Ϊһֱ��ʾ
                UIbar.gameObject.SetActive(false);//�ر���ʾѪ��
            else
                timeLeft -= Time.deltaTime;//����ʣ��ʱ��
        }
    }
}
