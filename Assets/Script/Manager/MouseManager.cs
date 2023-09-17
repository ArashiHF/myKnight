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
    

    public Texture2D point, doorway, attack, target, arrow;//鼠标指针图片:可点，传送门，攻击目标，移动目标，箭头

    RaycastHit hitInfo;//摄像头碰撞物体信息

    public event Action<Vector3> OnMouseClicked;//鼠标点击的坐标
    public event Action<GameObject> OnEnemyClicked;//点击的是敌人



    protected override void Awake()//当换场景的话不销毁
    {
        base.Awake();//继承
        DontDestroyOnLoad(this);//不要删除
    }

    void Update()
    {
        SetCursoTexture();
        if (InteractWithUI()) return;//如果点击UI则不要移动
        MouseControl();
    }

    void SetCursoTexture()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//做出射线
        if(InteractWithUI())
        {
            Cursor.SetCursor(point, Vector2.zero, CursorMode.Auto);//不做偏移
            return;
        }
        if(Physics.Raycast(ray,out hitInfo))
        {
            //切换鼠标图标
            switch (hitInfo.collider.gameObject.tag)
            {
                case "Ground":
                    Cursor.SetCursor(target,new Vector2(16,16),CursorMode.Auto);//图片偏移16,16
                    break;
                case "Enemy":
                    Cursor.SetCursor(attack, new Vector2(16, 16), CursorMode.Auto);//图片偏移16,16
                    break;
                case "Portal":
                    Cursor.SetCursor(doorway, new Vector2(16, 16), CursorMode.Auto);//图片偏移16,16
                    break;
                case "Item":
                    Cursor.SetCursor(point, Vector2.zero, CursorMode.Auto);//不做偏移
                    break;
                default:
                    Cursor.SetCursor(arrow, new Vector2(16, 16), CursorMode.Auto);//图片偏移16,16
                    break;
            }

        }
    }

    void MouseControl()
    {
        if(Input.GetMouseButtonDown(0)&&hitInfo.collider != null)
        {
            if (hitInfo.collider.gameObject.CompareTag("Ground"))//点击的是地板就走过去
                OnMouseClicked?.Invoke(hitInfo.point);
            if (hitInfo.collider.gameObject.CompareTag("Enemy"))
                OnEnemyClicked?.Invoke(hitInfo.collider.gameObject);//点击的是敌人时也走过去
            if (hitInfo.collider.gameObject.CompareTag("Attackable"))
                OnEnemyClicked?.Invoke(hitInfo.collider.gameObject);//点击的是敌人时也走过去
            if (hitInfo.collider.gameObject.CompareTag("Portal"))//点击的是传送门
                OnMouseClicked?.Invoke(hitInfo.point);
            if (hitInfo.collider.gameObject.CompareTag("Item"))//点击的是传送门
                OnMouseClicked?.Invoke(hitInfo.point);
        }
    }

    bool InteractWithUI()//是否点击UI
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())//如果点击的是ui则停止动作
        {
            return true;
        }
        else return false;
    }
}
