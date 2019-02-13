using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class TennisPlayerController : NetworkBehaviour
{
    private GameManager gameManager;

    [SerializeField]
    private float speed;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private GameObject tennisBallPrefab;

    private Rigidbody rb;

    private NetworkClient client;

    private int id;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        client = NetworkManager.singleton.client;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        this.id = this.gameManager.CreateNewId();
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

        if (Input.GetButtonDown("Fire1") && !gameManager.PointInProgress) {
            CmdBallService(this.id);
            this.client.Send(1002, new StringMessage("Service"));
        }
    }

    public int GetId() {
        return id;
    }

    [Command]
    void CmdBallService(int playerId) {
        GameObject tennisBall = (GameObject)Instantiate(tennisBallPrefab,
            transform.position + transform.forward + transform.up, Quaternion.identity);

        Rigidbody rb = tennisBall.GetComponent<Rigidbody>();
        rb.velocity = transform.up * 5f + transform.forward * 10f;

        gameManager.Service(playerId);

        NetworkServer.Spawn(tennisBall);

    }

}
