using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyLogger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("I'm in Start from a Script");
        int result = Sum(10, 5);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("I'm in Update from a Script");
    }

    public int Sum(int a, int b)
    {
        return a + b;
    }
}
