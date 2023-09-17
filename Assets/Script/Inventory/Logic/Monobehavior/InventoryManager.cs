using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : Singleton<InventoryManager>
{

    public class DragData
    {
        public SlotHolder originalHolder;//最开始移动前的格子位置

        public RectTransform originalParent;//最开始的父类
    }
    //最后添加存储数据
    [Header("Inventory Data")]
    public InventoryData_SO inventoryTempLate;//背包数据

    public InventoryData_SO inventoryData;//背包保存数据

    [Header("Action Data")]
    public InventoryData_SO actionTempLate;//快捷栏数据

    public InventoryData_SO actionData;//快捷栏保存数据

    [Header("Equipment Data")]
    public InventoryData_SO equipmentTempLate;//状态栏保存数据

    public InventoryData_SO equipmentData;//装备数据




    [Header("ContainerS")]
    public ContainerUI inventoryUI;//获取排列好的格子数据

    public ContainerUI actionUI;//快捷栏格子数据

    public ContainerUI equipmentUI;//装备栏格子数据

    [Header("Drag Canvas")]
    public Canvas dragCanvas;

    public DragData currentDrag;

    bool isOpen = false;//装备和状态栏开关

    [Header("UI Panel")]
    public GameObject bagPanel;//背包

    public GameObject statsPanel;//状态栏

    [Header("Stats Text")]
    public Text healthText;//血量

    public Text attackText;//攻击力

    [Header("Tooltip")]
    public ItemTooltip tooltip;//简介

    protected override void Awake()
    {
        base.Awake();
        //如果数据没有被清空，也就是有存档
        if (inventoryTempLate != null)
            inventoryData = inventoryTempLate;//获取背包数据
        if (actionTempLate != null)
            actionData = actionTempLate;//获取快捷键数据
        if (equipmentTempLate != null)
            equipmentData = equipmentTempLate;//获取装备栏数据
    }

    void Start()
    {
        //启动时载入数据
        LoadData();
        //开启的时候要刷新一次所有的数据
        inventoryUI.RefreshUI();
        actionUI.RefreshUI();
        equipmentUI.RefreshUI();
    }

    void Update()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        if (Input.GetKeyDown(KeyCode.B))//开关背包
        {
            isOpen = !isOpen;
            bagPanel.SetActive(isOpen);
            statsPanel.SetActive(isOpen);
        }

        UpdataStatsText(GameManager.Instance.playerStats.MaxHealth, GameManager.Instance.playerStats.attackData.minDamage,
            GameManager.Instance.playerStats.attackData.maxDamage);//更新面板数据
    }

    public void SaveData()
    {
        //保存数据,保存方式只是存下ScriptObject
        SaveManager.Instance.Save(inventoryData, inventoryData.name);
        SaveManager.Instance.Save(actionData, actionData.name);
        SaveManager.Instance.Save(equipmentData, equipmentData.name);
    }
    public void LoadData()
    {
        //载入数据
        SaveManager.Instance.Load(inventoryData, inventoryData.name);
        SaveManager.Instance.Load(actionData, actionData.name);
        SaveManager.Instance.Load(equipmentData, equipmentData.name);
    }

    public void UpdataStatsText(int health,int min ,int max)//更新血量和攻击力面板
    {
        healthText.text = health.ToString();
        attackText.text = min + " - " + max;
    }

    //判断格子的类型

    #region 判断拖拽的物品是否在各自范围内
    public bool CheckInInventoryUI(Vector3 position)
    {
        for(int i = 0;i<inventoryUI.slotHolders.Length;i++)
        {
            RectTransform t = inventoryUI.slotHolders[i].transform as RectTransform;//如果两个位置相同则做判定

            if(RectTransformUtility.RectangleContainsScreenPoint(t,position))//判定格子是否相同
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
            RectTransform t = actionUI.slotHolders[i].transform as RectTransform;//如果两个位置相同则做判定

            if (RectTransformUtility.RectangleContainsScreenPoint(t, position))//判定格子是否相同
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
            RectTransform t = equipmentUI.slotHolders[i].transform as RectTransform;//如果两个位置相同则做判定

            if (RectTransformUtility.RectangleContainsScreenPoint(t, position))//判定格子是否相同
            {
                return true;
            }
        }
        return false;
    }

    #endregion
    #region check Bag & Action
    public void CheckQuestItemInBag(string questItemName)//检查背包和快捷栏是否有任务的物品
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
        return inventoryData.items.Find(i => i.itemData == questItem);//获取背包数据中是否有这种东西
    }

    public InventoryItem QuestItemInAction(ItemData_SO questItem)
    {
        return actionData.items.Find(i => i.itemData == questItem);//获取快捷栏中是否有这种东西
    }
}
