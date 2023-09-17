using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionButton : MonoBehaviour
{
    public KeyCode actionKey;//��ݼ�

    public SlotHolder currentSlotHolder;//��ǰ�����

    void Awake()
    {
        currentSlotHolder = GetComponent<SlotHolder>();//��ȡ����
    }

    void Update()
    {
        if(Input.GetKeyDown(actionKey)&&currentSlotHolder.itemUI.GetItem())//���ݰ������Ұ�����Ϊ��
        {
            currentSlotHolder.UseItem();//ʹ����Ʒ
        }
    }
}
