using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public GameObject healthUIPrefab;//血条预制体

    public Transform barPoint;//血条位置

    public bool alwayVisible;//长久可见，可以设计成是否长久可见

    public float visibleTime;//可见时间

    private float timeLeft;//剩余显示时间

    Image healthSlider;//血条

    Transform UIbar;//UI坐标

    Transform cam;//相机位置，让血条对准相机

    CharacterStats currentStats;//当前状态

    void Awake()
    {
        currentStats = GetComponent<CharacterStats>();//获得当前数据

        currentStats.UpdataHealthBarOnAttack += UpdateHealthBar; //添加更新血条函数
    }

    void OnEnable()
    {
        cam = Camera.main.transform;//获取相机位置

        foreach(Canvas canvas in FindObjectsOfType<Canvas>())//遍历所有canvas
        {
            if(canvas.renderMode == RenderMode.WorldSpace)//如果为worldSpace
            {
                UIbar = Instantiate(healthUIPrefab, canvas.transform).transform;//获取当前的摄像机位置

                healthSlider = UIbar.GetChild(0).GetComponent<Image>();//获得血条

                UIbar.gameObject.SetActive(alwayVisible);//平常确定是否可见
            }
        }
    }

    private void UpdateHealthBar(int currentHealth, int maxHealth)//更新血条
    {
        if (currentHealth <= 0)
            Destroy(UIbar.gameObject);//删除血条
        UIbar.gameObject.SetActive(true);//被打了就可见
        timeLeft = visibleTime;
        float sliderPercent = (float)currentHealth / maxHealth;//血条百分比制
        healthSlider.fillAmount = sliderPercent;//更新血条
    }

    void LateUpdate()
    {
        if(UIbar != null)
        {
            UIbar.position = barPoint.position;//更新血条坐标
            UIbar.forward = -cam.forward;//血条时刻朝着摄像机，相机的反方向为血条方向

            if (timeLeft <= 0 && !alwayVisible)//如果还有剩余时间并且不为一直显示
                UIbar.gameObject.SetActive(false);//关闭显示血条
            else
                timeLeft -= Time.deltaTime;//减少剩余时间
        }
    }
}
