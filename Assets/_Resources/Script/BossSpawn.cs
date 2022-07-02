using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawn : MonoBehaviour
{
    [SerializeField] private Transform boss;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            boss.gameObject.SetActive(true);
            Destroy(gameObject);
        }
    }
}
