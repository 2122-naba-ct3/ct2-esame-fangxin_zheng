using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] private Transform fxBoom;
    public float speed = 50f;
    private Rigidbody rig;
    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        rig.velocity = transform.forward * speed;
        Destroy(gameObject,5f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<ThirdPersonShooterController>().Hurt();
        }
        else
        {
            Instantiate(fxBoom, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
