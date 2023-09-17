using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemTooltip : MonoBehaviour
{
    public Text itemNameText;//简介名字

    public Text itemInfoText;//简介内容

    RectTransform rectTransform;//简介坐标

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();//获取简介坐标
    }

    public void SetUpTooltip(ItemData_SO item)//更新内容
    {
        itemNameText.text = item.itemName;
        itemInfoText.text = item.description;
    }

    void OnEnable()
    {
        UpdataPosition();//防止闪烁
    }

    void Update()
    {
        UpdataPosition();//更新简介坐标
    }

    public void UpdataPosition()//更新坐标
    {
        Vector3 mousePos = Input.mousePosition;//赋予鼠标坐标

        Vector3[] corners = new Vector3[4];//获取四个角的坐标从0-4，顺时针转动，从0开始为左下角 1左上 2右上  3右下
        rectTransform.GetWorldCorners(corners);//获取四角坐标


        float width = corners[3].x - corners[0].x;
        float height = corners[1].y - corners[0].y;//偏移的框和高

        if (mousePos.y < height)
            rectTransform.position = mousePos + Vector3.up * height * 0.6f;//如果位置大于高度
        else if (Screen.width - mousePos.x > width)
            rectTransform.position = mousePos + Vector3.right * width * 0.6f;//右边越界
        else
            rectTransform.position = mousePos + Vector3.left * width * 0.6f;//左边越界

    }
}
