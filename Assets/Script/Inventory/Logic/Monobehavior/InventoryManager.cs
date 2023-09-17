using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : Singleton<InventoryManager>
{

    public class DragData
    {
        public SlotHolder originalHolder;//�ʼ�ƶ�ǰ�ĸ���λ��

        public RectTransform originalParent;//�ʼ�ĸ���
    }
    //�����Ӵ洢����
    [Header("Inventory Data")]
    public InventoryData_SO inventoryTempLate;//��������

    public InventoryData_SO inventoryData;//������������

    [Header("Action Data")]
    public InventoryData_SO actionTempLate;//���������

    public InventoryData_SO actionData;//�������������

    [Header("Equipment Data")]
    public InventoryData_SO equipmentTempLate;//״̬����������

    public InventoryData_SO equipmentData;//װ������




    [Header("ContainerS")]
    public ContainerUI inventoryUI;//��ȡ���кõĸ�������

    public ContainerUI actionUI;//�������������

    public ContainerUI equipmentUI;//װ������������

    [Header("Drag Canvas")]
    public Canvas dragCanvas;

    public DragData currentDrag;

    bool isOpen = false;//װ����״̬������

    [Header("UI Panel")]
    public GameObject bagPanel;//����

    public GameObject statsPanel;//״̬��

    [Header("Stats Text")]
    public Text healthText;//Ѫ��

    public Text attackText;//������

    [Header("Tooltip")]
    public ItemTooltip tooltip;//���

    protected override void Awake()
    {
        base.Awake();
        //�������û�б���գ�Ҳ�����д浵
        if (inventoryTempLate != null)
            inventoryData = inventoryTempLate;//��ȡ��������
        if (actionTempLate != null)
            actionData = actionTempLate;//��ȡ��ݼ�����
        if (equipmentTempLate != null)
            equipmentData = equipmentTempLate;//��ȡװ��������
    }

    void Start()
    {
        //����ʱ��������
        LoadData();
        //������ʱ��Ҫˢ��һ�����е�����
        inventoryUI.RefreshUI();
        actionUI.RefreshUI();
        equipmentUI.RefreshUI();
    }

    void Update()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        if (Input.GetKeyDown(KeyCode.B))//���ر���
        {
            isOpen = !isOpen;
            bagPanel.SetActive(isOpen);
            statsPanel.SetActive(isOpen);
        }

        UpdataStatsText(GameManager.Instance.playerStats.MaxHealth, GameManager.Instance.playerStats.attackData.minDamage,
            GameManager.Instance.playerStats.attackData.maxDamage);//�����������
    }

    public void SaveData()
    {
        //��������,���淽ʽֻ�Ǵ���ScriptObject
        SaveManager.Instance.Save(inventoryData, inventoryData.name);
        SaveManager.Instance.Save(actionData, actionData.name);
        SaveManager.Instance.Save(equipmentData, equipmentData.name);
    }
    public void LoadData()
    {
        //��������
        SaveManager.Instance.Load(inventoryData, inventoryData.name);
        SaveManager.Instance.Load(actionData, actionData.name);
        SaveManager.Instance.Load(equipmentData, equipmentData.name);
    }

    public void UpdataStatsText(int health,int min ,int max)//����Ѫ���͹��������
    {
        healthText.text = health.ToString();
        attackText.text = min + " - " + max;
    }

    //�жϸ��ӵ�����

    #region �ж���ק����Ʒ�Ƿ��ڸ��Է�Χ��
    public bool CheckInInventoryUI(Vector3 position)
    {
        for(int i = 0;i<inventoryUI.slotHolders.Length;i++)
        {
            RectTransform t = inventoryUI.slotHolders[i].transform as RectTransform;//�������λ����ͬ�����ж�

            if(RectTransformUtility.RectangleContainsScreenPoint(t,position))//�ж������Ƿ���ͬ
            {
                return true;
            }
        }
        return false;
    }

    public bool CheckInActionUI(Vector3 position)
    {
        for (int i = 0; i < actionUI.slotHolders.Length; i++)
        {
            RectTransform t = actionUI.slotHolders[i].transform as RectTransform;//�������λ����ͬ�����ж�

            if (RectTransformUtility.RectangleContainsScreenPoint(t, position))//�ж������Ƿ���ͬ
            {
                return true;
            }
        }
        return false;
    }

    public bool CheckInEquipmentUI(Vector3 position)
    {
        for (int i = 0; i < equipmentUI.slotHolders.Length; i++)
        {
            RectTransform t = equipmentUI.slotHolders[i].transform as RectTransform;//�������λ����ͬ�����ж�

            if (RectTransformUtility.RectangleContainsScreenPoint(t, position))//�ж������Ƿ���ͬ
            {
                return true;
            }
        }
        return false;
    }

    #endregion
    #region check Bag & Action
    public void CheckQuestItemInBag(string questItemName)//��鱳���Ϳ�����Ƿ����������Ʒ
    {
        foreach(var item in inventoryData.items)
        {
            if (item.itemData != null)
            {
                if (item.itemData.itemName == questItemName)
                    QuestManager.Instance.UpdateQuestProgress(item.itemData.itemName,item.amount);
            }
        }
    }
    #endregion

    public InventoryItem QuestItemInBag(ItemData_SO questItem)
    {
        return inventoryData.items.Find(i => i.itemData == questItem);//��ȡ�����������Ƿ������ֶ���
    }

    public InventoryItem QuestItemInAction(ItemData_SO questItem)
    {
        return actionData.items.Find(i => i.itemData == questItem);//��ȡ��������Ƿ������ֶ���
    }
}
