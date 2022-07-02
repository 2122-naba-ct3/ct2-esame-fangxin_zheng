using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpwan : MonoBehaviour
{
    public GameObject[] enemy;
    public int enemyIndex;  //敌人种类
    public int spawnNum;    //敌人数量
    public float spawnTime; //生成间隔
    private bool working = false;


    private void OnTriggerEnter(Collider other)
    {
        if (!working && other.tag == "Player")
        {
            working = true;
            StartCoroutine(Spwaning());
        }
    }

    IEnumerator Spwaning()
    {
        for (int i = spawnNum; i > 0; i--)
        {
            Instantiate(enemy[enemyIndex],transform.GetChild(0).position + new Vector3(0f, 1f, 0f), transform.GetChild(0).rotation);
            yield return new WaitForSeconds(spawnTime);
        }

        Destroy(gameObject);
    }
}
