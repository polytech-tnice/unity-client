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


  // Update is called once per frame
  void Update()
  {

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
