using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemTooltip : MonoBehaviour
{
    public Text itemNameText;//�������

    public Text itemInfoText;//�������

    RectTransform rectTransform;//�������

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();//��ȡ�������
    }

    public void SetUpTooltip(ItemData_SO item)//��������
    {
        itemNameText.text = item.itemName;
        itemInfoText.text = item.description;
    }

    void OnEnable()
    {
        UpdataPosition();//��ֹ��˸
    }

    void Update()
    {
        UpdataPosition();//���¼������
    }

    public void UpdataPosition()//��������
    {
        Vector3 mousePos = Input.mousePosition;//�����������

        Vector3[] corners = new Vector3[4];//��ȡ�ĸ��ǵ������0-4��˳ʱ��ת������0��ʼΪ���½� 1���� 2����  3����
        rectTransform.GetWorldCorners(corners);//��ȡ�Ľ�����


        float width = corners[3].x - corners[0].x;
        float height = corners[1].y - corners[0].y;//ƫ�ƵĿ�͸�

        if (mousePos.y < height)
            rectTransform.position = mousePos + Vector3.up * height * 0.6f;//���λ�ô��ڸ߶�
        else if (Screen.width - mousePos.x > width)
            rectTransform.position = mousePos + Vector3.right * width * 0.6f;//�ұ�Խ��
        else
            rectTransform.position = mousePos + Vector3.left * width * 0.6f;//���Խ��

    }
}
