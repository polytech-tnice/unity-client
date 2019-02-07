using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TennisPlayerController : NetworkBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private GameObject tennisBallPrefab;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) {
            cam.enabled = false;
            return;
        }
        
        Vector3 localVelocity = transform.InverseTransformDirection(rb.velocity);

        localVelocity.x = Input.GetAxis("Horizontal") * speed;
        localVelocity.y = 0;
        localVelocity.z = Input.GetAxis("Vertical") * speed;
                
        rb.velocity = transform.TransformDirection(localVelocity);

        if (Input.GetButtonDown("Fire1")) {
            CmdBallService();
        }
    }

    [Command]
    void CmdBallService() {
        GameObject tennisBall = (GameObject)Instantiate(tennisBallPrefab,
            transform.position + transform.forward + transform.up, Quaternion.identity);

        Rigidbody rb = tennisBall.GetComponent<Rigidbody>();
        rb.velocity = transform.up * 5f + transform.forward * 10f;

        NetworkServer.Spawn(tennisBall);
    }
}
