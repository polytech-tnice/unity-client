using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using SocketIO;

public class CustomNetManager : NetworkManager
{
    [SerializeField]
    private SocketIOComponent socket;

    public void InitGameServer() {
        StartServer();
    }

    public void ConnectClientToServer(InputField serverIpField) {
        networkAddress = serverIpField.text;

        StartClient();
    }
}
