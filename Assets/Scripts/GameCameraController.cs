using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameCameraController : NetworkBehaviour
{

  [SerializeField]
  private float speed;
  private Camera cam;
  private Rigidbody body;
  private Vector3 pos = Vector3.zero;
  public float lookSenitivity = 4f;
  public float lookSmoothDamp = 0.1f;
  public float xRotation;
  public float yRotation;
  public float xRotationV;
  public float yRotationV;
  public float currentXRotation;
  public float currentYRotation;
  [HideInInspector]
  public float lStickHorizontal;
  [HideInInspector]
  public float lStickVertical;
  public float lStickV;
  public float lStickH;

  private GameObject[] cameras;

  private int currentCamera;

  // Start is called before the first frame update
  void Start()
  {
    cam = GetComponent<Camera>();
    currentCamera = -1;
    cameras = GameObject.FindGameObjectsWithTag("FixedCamera");
    body = GetComponent<Rigidbody>();
  }


  // Update is called once per frame
  void Update()
  {
    if (!isLocalPlayer)
    {
      cam.enabled = false;
      return;
    }

    if (Input.GetKeyDown(KeyCode.Joystick1Button8))
    {
      switchCameraRight();
    }

    if (Input.GetKeyDown(KeyCode.Joystick1Button7))
    {
      switchCameraLeft();
    }

    if (Input.GetKeyDown(KeyCode.Joystick1Button1) && currentCamera != -1)
    {
      cameras[currentCamera].GetComponent<Camera>().enabled = false;
      cam.enabled = true;
      currentCamera = -1;
    }


    // If not Main Camera, don't move or rotate
    if (currentCamera != -1)
    {
      return;
    }

    lStickHorizontal = Input.GetAxis("LookY");
    lStickVertical = Input.GetAxis("LookX");

    lStickH = lStickHorizontal;
    lStickV = lStickVertical;

    xRotation += Input.GetAxis("LookX") * lookSenitivity;
    yRotation += Input.GetAxis("LookY") * lookSenitivity;

    xRotation = Mathf.Clamp(xRotation, -90, 90);

    currentXRotation = Mathf.SmoothDamp(currentXRotation, xRotation, ref xRotationV, lookSmoothDamp);
    currentYRotation = Mathf.SmoothDamp(currentYRotation, yRotation, ref yRotationV, lookSmoothDamp);

    transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);

    var direction = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
    transform.Translate(direction * speed * Time.deltaTime, Space.Self);
  }

  private void showInputs()
  {
    System.Array values = System.Enum.GetValues(typeof(KeyCode));
    foreach (KeyCode code in values)
    {
      if (Input.GetKeyDown(code)) { print(System.Enum.GetName(typeof(KeyCode), code)); }
    }
  }

  private void switchCameraRight()
  {
    if (currentCamera == -1)
    {
      cam.enabled = false;
      cameras[0].GetComponent<Camera>().enabled = true;
      currentCamera = 0;
      return;
    }

    if (currentCamera == cameras.Length - 1)
    {
      cam.enabled = true;
      cameras[cameras.Length - 1].GetComponent<Camera>().enabled = false;
      currentCamera = -1;
      return;
    }

    cameras[currentCamera].GetComponent<Camera>().enabled = false;
    cameras[++currentCamera].GetComponent<Camera>().enabled = true;
  }

  private void switchCameraLeft()
  {
    if (currentCamera == -1)
    {
      cam.enabled = false;
      cameras[cameras.Length - 1].GetComponent<Camera>().enabled = true;
      currentCamera = cameras.Length - 1;
      return;
    }

    if (currentCamera == 0)
    {
      cam.enabled = true;
      cameras[0].GetComponent<Camera>().enabled = false;
      currentCamera = -1;
      return;
    }

    cameras[currentCamera].GetComponent<Camera>().enabled = false;
    cameras[--currentCamera].GetComponent<Camera>().enabled = true;
  }

}
