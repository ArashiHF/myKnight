using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionButton : MonoBehaviour
{
    public KeyCode actionKey;//快捷键

    public SlotHolder currentSlotHolder;//当前快捷栏

    void Awake()
    {
        currentSlotHolder = GetComponent<SlotHolder>();//获取格子
    }

    void Update()
    {
        if(Input.GetKeyDown(actionKey)&&currentSlotHolder.itemUI.GetItem())//根据按键而且按键不为空
        {
            currentSlotHolder.UseItem();//使用物品
        }
    }
}
