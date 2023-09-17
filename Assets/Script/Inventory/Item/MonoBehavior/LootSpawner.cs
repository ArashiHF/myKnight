using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootSpawner : MonoBehaviour
{
    [System.Serializable]
    public class LootItem
    {
        public GameObject item;

        [Range(0,1)]
        public float weight;
    }

    public LootItem[] lootItems;

    public void SpawnLoot()
    {
        float currentValue = Random.value;//概率随机数

        for (int i = 0; i < lootItems.Length;i++)//循环所有的东西
        {
            if(currentValue <= lootItems[i].weight)//如果这个生成的随机数小于概率
            {
                GameObject obj = Instantiate(lootItems[i].item);//生成掉落物
                obj.transform.position = transform.position + Vector3.up * 2;//给掉落物加一个高度
                //break; //只掉落一个
            }
        }
    }

}
