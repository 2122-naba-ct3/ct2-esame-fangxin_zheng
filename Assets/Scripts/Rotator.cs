using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{

    public float rotationSpeed = 3.5f;
    
    // Update is called once per frame
    void Update()
    {
        Vector3 rot =  transform.up *rotationSpeed * Time.deltaTime;
        transform.Rotate(rot, Space.World);
    }
}
