using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class OVRRacket : MonoBehaviour
{
    [SerializeField]
    private TennisPlayerController player;

    [SerializeField]
    private Transform anchor;

    private Quaternion initRotation;

    // Start is called before the first frame update
    void Start()
    {
        initRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.isLocalPlayer) {
            return;
        }

        if (ControllerIsConnected) {
            Vector3 position = OVRInput.GetLocalControllerPosition(Controller) + anchor.localPosition;
            Quaternion rotation =  OVRInput.GetLocalControllerRotation (Controller) * initRotation;

            transform.localPosition = position;
            transform.localRotation = rotation;
        }
    }

    public bool ControllerIsConnected
    {
        get
        {
            OVRInput.Controller controller = OVRInput.GetConnectedControllers() & (OVRInput.Controller.LTrackedRemote | OVRInput.Controller.RTrackedRemote);
            return controller == OVRInput.Controller.LTrackedRemote || controller == OVRInput.Controller.RTrackedRemote;
        }
    }
    public OVRInput.Controller Controller
    {
        get
        {
            OVRInput.Controller controller = OVRInput.GetConnectedControllers();
            if ((controller & OVRInput.Controller.LTrackedRemote) == OVRInput.Controller.LTrackedRemote)
            {
                return OVRInput.Controller.LTrackedRemote;
            }
            else if ((controller & OVRInput.Controller.RTrackedRemote) == OVRInput.Controller.RTrackedRemote)
            {
                return OVRInput.Controller.RTrackedRemote;
            }
            return OVRInput.GetActiveController();
        }
    }
}