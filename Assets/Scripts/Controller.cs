using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class Controller : NetworkBehaviour
{

    void Start()
    {
        Input.gyro.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, -Input.gyro.rotationRateUnbiased.y, 0);
        transform.Rotate(-Input.gyro.rotationRateUnbiased.x, 0, 0);
        transform.Rotate(0, 0, -Input.gyro.rotationRateUnbiased.z);
    }
}
