using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using UnityEngine.EventSystems;

/*[System.Serializable]
public class EventVector3 : UnityEvent<Vector3> { }*/
public class MouseManager : Singleton<MouseManager>
{
    

    public Texture2D point, doorway, attack, target, arrow;//���ָ��ͼƬ:�ɵ㣬�����ţ�����Ŀ�꣬�ƶ�Ŀ�꣬��ͷ

    RaycastHit hitInfo;//����ͷ��ײ������Ϣ

    public event Action<Vector3> OnMouseClicked;//�����������
    public event Action<GameObject> OnEnemyClicked;//������ǵ���



    protected override void Awake()//���������Ļ�������
    {
        base.Awake();//�̳�
        DontDestroyOnLoad(this);//��Ҫɾ��
    }

    void Update()
    {
        SetCursoTexture();
        if (InteractWithUI()) return;//������UI��Ҫ�ƶ�
        MouseControl();
    }

    void SetCursoTexture()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//��������
        if(InteractWithUI())
        {
            Cursor.SetCursor(point, Vector2.zero, CursorMode.Auto);//����ƫ��
            return;
        }
        if(Physics.Raycast(ray,out hitInfo))
        {
            //�л����ͼ��
            switch (hitInfo.collider.gameObject.tag)
            {
                case "Ground":
                    Cursor.SetCursor(target,new Vector2(16,16),CursorMode.Auto);//ͼƬƫ��16,16
                    break;
                case "Enemy":
                    Cursor.SetCursor(attack, new Vector2(16, 16), CursorMode.Auto);//ͼƬƫ��16,16
                    break;
                case "Portal":
                    Cursor.SetCursor(doorway, new Vector2(16, 16), CursorMode.Auto);//ͼƬƫ��16,16
                    break;
                case "Item":
                    Cursor.SetCursor(point, Vector2.zero, CursorMode.Auto);//����ƫ��
                    break;
                default:
                    Cursor.SetCursor(arrow, new Vector2(16, 16), CursorMode.Auto);//ͼƬƫ��16,16
                    break;
            }

        }
    }

    void MouseControl()
    {
        if(Input.GetMouseButtonDown(0)&&hitInfo.collider != null)
        {
            if (hitInfo.collider.gameObject.CompareTag("Ground"))//������ǵذ���߹�ȥ
                OnMouseClicked?.Invoke(hitInfo.point);
            if (hitInfo.collider.gameObject.CompareTag("Enemy"))
                OnEnemyClicked?.Invoke(hitInfo.collider.gameObject);//������ǵ���ʱҲ�߹�ȥ
            if (hitInfo.collider.gameObject.CompareTag("Attackable"))
                OnEnemyClicked?.Invoke(hitInfo.collider.gameObject);//������ǵ���ʱҲ�߹�ȥ
            if (hitInfo.collider.gameObject.CompareTag("Portal"))//������Ǵ�����
                OnMouseClicked?.Invoke(hitInfo.point);
            if (hitInfo.collider.gameObject.CompareTag("Item"))//������Ǵ�����
                OnMouseClicked?.Invoke(hitInfo.point);
        }
    }

    bool InteractWithUI()//�Ƿ���UI
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())//����������ui��ֹͣ����
        {
            return true;
        }
        else return false;
    }
}
