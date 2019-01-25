using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMovementScript : MonoBehaviour
{
  // Start is called before the first frame update
  Rigidbody ourDrone;

  void Awake()
  {
    ourDrone = GetComponent<Rigidbody>();
  }

  void FixedUpdate()
  {
    MovementUpDown();

    ourDrone.AddRelativeForce(Vector3.up * upForce);
  }

  public float upForce;
  void MovementUpDown()
  {
    if (Input.GetKey(KeyCode.I))
    {
      upForce = 450;
    }
    else if (Input.GetKey(KeyCode.K))
    {
      upForce = -200;
    }
    else if (!Input.GetKey(KeyCode.I) && !Input.GetKey(KeyCode.K))
    {
      upForce = 98.1f;
    }
  }

}
