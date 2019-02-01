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
  public float lookSenitivity = 0.5f;
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

  // Start is called before the first frame update
  void Start()
  {
    cam = GetComponent<Camera>();
    body = GetComponent<Rigidbody>();
  }


  // Update is called once per frame
  void Update()
  {
    if (!isLocalPlayer) {
      Debug.Log("Not quite my local player");
      cam.enabled = false;
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

    float newX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
    float newZ = Input.GetAxis("Vertical") * speed * Time.deltaTime;

    var direction = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
    transform.Translate(direction * speed * Time.deltaTime, Space.Self);
  }

}
