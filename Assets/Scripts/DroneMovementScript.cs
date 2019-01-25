using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMovementScript : MonoBehaviour
{
  // Our Drone
  Rigidbody ourDrone;

  // Vertical Movement
  float vertical;
  // Horizontal Movement
  float horizontal;
  // UP Movement
  bool upDrone;
  // DOWN Movement
  bool downDrone;
  // Rotate Left Movement
  bool rotateLeftDrone;
  // Rotate Right Movement
  bool rotateRightDrone;



  void Awake()
  {
    ourDrone = GetComponent<Rigidbody>();

  }

  void FixedUpdate()
  {
    vertical = Input.GetAxis("Vertical");
    horizontal = Input.GetAxis("Horizontal");
    upDrone = Input.GetKey(KeyCode.I) || (Input.GetKey(KeyCode.Joystick1Button0) && vertical < 0);
    downDrone = Input.GetKey(KeyCode.K) || (Input.GetKey(KeyCode.Joystick1Button0) && vertical > 0);
    rotateLeftDrone = Input.GetKey(KeyCode.J) || Input.GetKey(KeyCode.Joystick1Button1);
    rotateRightDrone = Input.GetKey(KeyCode.L) || Input.GetKey(KeyCode.Joystick1Button2);

    MovementUpDown();
    MovementForward();
    Rotation();
    ClampingSpeedValues();
    Swerve();

    ourDrone.AddRelativeForce(Vector3.up * upForce);
    ourDrone.rotation = Quaternion.Euler(
      new Vector3(tiltAmountForward, currentYRotation, ourDrone.rotation.z)
    );
  }

  public float upForce;
  void MovementUpDown()
  {


    if (Mathf.Abs(vertical) > 0.2f || Mathf.Abs(horizontal) > 0.2f)
    {
      if (upDrone || downDrone)
      {
        ourDrone.velocity = ourDrone.velocity;
      }
      if (!upDrone && !downDrone && !rotateLeftDrone && !rotateRightDrone)
      {
        ourDrone.velocity = new Vector3(ourDrone.velocity.x, Mathf.Lerp(ourDrone.velocity.y, 0, Time.deltaTime * 5), ourDrone.velocity.z);
        upForce = 281;
      }
      if (!upDrone && !downDrone && (rotateLeftDrone || rotateRightDrone))
      {
        ourDrone.velocity = new Vector3(ourDrone.velocity.x, Mathf.Lerp(ourDrone.velocity.y, 0, Time.deltaTime * 5), ourDrone.velocity.z);
        upForce = 110;
      }
      if (rotateLeftDrone || rotateRightDrone)
      {
        upForce = 410;
      }
    }

    if (Mathf.Abs(vertical) < 0.2f && Mathf.Abs(horizontal) > 0.2f)
    {
      upForce = 135;
    }

    if (upDrone)
    {
      upForce = 450;
      if (Mathf.Abs(horizontal) > 0.2f)
      {
        upForce = 500;
      }
    }
    else if (downDrone)
    {
      upForce = -200;
    }
    else if (!upDrone && !downDrone && (Mathf.Abs(vertical) < 0.2f && Mathf.Abs(horizontal) < 0.2f))
    {
      upForce = 98.1f;
    }
  }

  private float movementForwardSpeed = 300.0f;
  private float tiltAmountForward = 0;
  private float tiltVelocityForward;

  void MovementForward()
  {
    if (vertical != 0 && !upDrone && !downDrone)
    {
      ourDrone.AddRelativeForce(Vector3.forward * vertical * movementForwardSpeed);
      tiltAmountForward = Mathf.SmoothDamp(tiltAmountForward, 20 * vertical, ref tiltVelocityForward, 0.1f);
    }
  }

  private float wantedYRotation;
  [HideInInspector] public float currentYRotation;
  private float rotateAmountByKeys = 2.5f;
  private float rotationYVelocity;
  void Rotation()
  {
    if (rotateLeftDrone)
    {
      wantedYRotation -= rotateAmountByKeys;
    }
    if (rotateRightDrone)
    {
      wantedYRotation += rotateAmountByKeys;
    }

    currentYRotation = Mathf.SmoothDamp(currentYRotation, wantedYRotation, ref rotationYVelocity, 0.25f);
  }


  private Vector3 velocityToSmoothDampToZero;
  void ClampingSpeedValues()
  {
    if (Mathf.Abs(vertical) > 0.2f && Mathf.Abs(horizontal) > 0.2f)
    {
      ourDrone.velocity = Vector3.ClampMagnitude(ourDrone.velocity, Mathf.Lerp(ourDrone.velocity.magnitude, 10.0f, Time.deltaTime * 5f));
    }
    if (Mathf.Abs(vertical) > 0.2f && Mathf.Abs(horizontal) < 0.2f)
    {
      ourDrone.velocity = Vector3.ClampMagnitude(ourDrone.velocity, Mathf.Lerp(ourDrone.velocity.magnitude, 10.0f, Time.deltaTime * 5f));
    }
    if (Mathf.Abs(vertical) < 0.2f && Mathf.Abs(horizontal) > 0.2f)
    {
      ourDrone.velocity = Vector3.ClampMagnitude(ourDrone.velocity, Mathf.Lerp(ourDrone.velocity.magnitude, 5.0f, Time.deltaTime * 5f));
    }
    if (Mathf.Abs(vertical) < 0.2f && Mathf.Abs(horizontal) < 0.2f)
    {
      ourDrone.velocity = Vector3.SmoothDamp(ourDrone.velocity, Vector3.zero, ref velocityToSmoothDampToZero, 0.95f);
    }
  }

  private float sideMovementAmount = 300.0f;
  private float tiltAmountSideways;
  private float tiltAmountVelocity;
  void Swerve()
  {
    if (Mathf.Abs(horizontal) > 0.2f)
    {
      ourDrone.AddRelativeForce(Vector3.right * horizontal * sideMovementAmount);
      tiltAmountSideways = Mathf.SmoothDamp(tiltAmountSideways, -20 * horizontal, ref tiltAmountVelocity, 0.1f);
    }
    else
    {
      tiltAmountSideways = Mathf.SmoothDamp(tiltAmountSideways, 0, ref tiltAmountVelocity, 0.1f);
    }
  }

}
