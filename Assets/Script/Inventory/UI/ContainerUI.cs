using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerUI : MonoBehaviour
{
    public SlotHolder[] slotHolders;//��������

    public void RefreshUI()//����UI������û�и���λ��
    {
        for(int i = 0;i<slotHolders.Length;i++)
        {
            slotHolders[i].itemUI.index = i;
            slotHolders[i].UpdateItem();
        }
    }
}
