using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{


    public GameObject objBullet;
    public Transform bulletSpwanPos;

    [SerializeField] private float shootIntervalTime;
    private float shootTimer = 0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        shootTimer += Time.deltaTime;
        if (shootTimer >= shootIntervalTime)
        {
            Vector3 aimDir = (GetComponent<Enemy>().player.position + new Vector3(0f, 1f, 0f) - bulletSpwanPos.position).normalized;
            Instantiate(objBullet, bulletSpwanPos.position, Quaternion.LookRotation(aimDir, Vector3.up));
            shootTimer = 0f;
        }
    }
}
