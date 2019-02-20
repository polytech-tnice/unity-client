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
    private GameObject[] cameras;

    [SerializeField]
    private GameObject tennisBallPrefab;

    [SerializeField]
    private Canvas waitCanvas;

    private Rigidbody rb;

    private NetworkClient client;

    private int id;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        client = NetworkManager.singleton.client;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        NetworkManager.singleton.client.connection.RegisterHandler(2000, OnRecieveId);
    }

    public override void OnStartAuthority() {
        CmdGenerateId();
    }


    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) {
            foreach (GameObject c in cameras)
            {
                c.SetActive(false);
            }
            waitCanvas.enabled = false;
            return;
        }
        
        waitCanvas.enabled = !gameManager.ReadyForPoint;

        if (gameManager.ReadyToPlay && gameManager.ReadyForPoint
            && OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger) && !gameManager.PointInProgress)
        {
            CmdBallService(this.id);
            this.client.Send(1002, new StringMessage("Service"));
        }
    }

    void FixedUpdate() {
        if (!isLocalPlayer) {
            return;
        }

        if (gameManager.ReadyToPlay && OVRInput.Get(OVRInput.Button.PrimaryTouchpad)) {
            Vector3 localVelocity = transform.InverseTransformDirection(rb.velocity);
            Vector2 movement = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);

            localVelocity.x = movement.x * speed;
            localVelocity.y = 0;
            localVelocity.z = movement.y * speed;
                    
            rb.velocity = transform.TransformDirection(localVelocity);
        }

    }

    public int GetId() {
        return id;
    }

    [Command]
    void CmdGenerateId() {
        this.id = this.gameManager.CreateNewId();
        IntegerMessage idMessage = new IntegerMessage(this.id);
        connectionToClient.Send(2000, idMessage);
    }

    [Command]
    void CmdBallService(int playerId) {
        GameObject tennisBall = (GameObject)Instantiate(tennisBallPrefab,
            transform.position + transform.forward + transform.up + transform.right, Quaternion.identity);

        Rigidbody rb = tennisBall.GetComponent<Rigidbody>();
        rb.velocity = transform.up * 7f;

        gameManager.Service(playerId);

        NetworkServer.Spawn(tennisBall);

    }

    void OnRecieveId(NetworkMessage netMsg) {
        IntegerMessage idMessage = netMsg.ReadMessage<IntegerMessage>();
        this.id = idMessage.value;
    }

}
