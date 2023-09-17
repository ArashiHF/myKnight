using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionPoint : MonoBehaviour
{
    public enum TransitionType//传送类型，通常将和不同场景
    {
        SameScene,DifferentScene
    }

    [Header("Transition Info")]
    public string sceneName;

    public TransitionType transitionType;//传送类型

    public TransitionDestination.DestinationTag destinationTag;//传送目的地

    private bool canTrans;//传送Trigger

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E)&&canTrans)//如果点击E而且在可传送位置
        {
            //进行传送
            SceneController.Instance.TransitionToDestination(this);
        }
    }

    void OnTriggerStay(Collider other)//玩家靠近传送门就可传送
    {
        if (other.CompareTag("Player"))
            canTrans = true;
    }
    void OnTriggerExit(Collider other)//玩家离开传送门,就关闭可传送
    {
        if (other.CompareTag("Player"))
            canTrans = false;
    }
}
