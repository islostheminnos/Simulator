using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float rotateSpeed = 10f;

    // Update is called once per frame
    void Update()
    {
        
        transform.Rotate(0,50*Time.deltaTime, 0);
    }
}
