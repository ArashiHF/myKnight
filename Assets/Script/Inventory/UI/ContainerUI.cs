using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerUI : MonoBehaviour
{
    public SlotHolder[] slotHolders;//各自数组

    public void RefreshUI()//更新UI，这里没有更新位置
    {
        for(int i = 0;i<slotHolders.Length;i++)
        {
            slotHolders[i].itemUI.index = i;
            slotHolders[i].UpdateItem();
        }
    }
}
