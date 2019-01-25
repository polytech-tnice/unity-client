using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCameraController : MonoBehaviour
{

  [SerializeField]
  private float speed;
  private Camera cam;
  private Vector3 pos;

  // Start is called before the first frame update
  void Start()
  {
    cam = GetComponent<Camera>();
  }


  float tiltAngle = 0.5f;
  // Update is called once per frame
  void Update()
  {
    if (Input.GetKey("joystick 1 button 0"))
    {
      float tiltAroundY = Input.GetAxis("Horizontal") * tiltAngle;
      float tiltAroundX = Input.GetAxis("Vertical") * tiltAngle;

      transform.Rotate(new Vector3(tiltAroundX, tiltAroundY, 0));
      return;
    }

    float newX = transform.position.x + Input.GetAxis("Horizontal") * speed * Time.deltaTime;
    float newZ = transform.position.z + Input.GetAxis("Vertical") * speed * Time.deltaTime;

    if (Input.GetKey("joystick 1 button 1"))
    {
      transform.position = new Vector3(newX, transform.position.y + speed * Time.deltaTime, newZ);
    }

    if (Input.GetKey("joystick 1 button 2"))
    {
      transform.position = new Vector3(newX, transform.position.y - speed * Time.deltaTime, newZ);
    }

    transform.position = new Vector3(newX, transform.position.y, newZ);

  }
}
