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
    }
}
