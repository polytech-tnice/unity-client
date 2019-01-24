using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SocketIO;

public class CustomNetManager : NetworkManager
{
    [SerializeField]
    private GameObject socketIOPrefab;

    private SocketIOComponent socket;
    private bool socketIOInstantiated = false;
    private bool launched = false;

    public void InitGameServer() {
        if (!socketIOInstantiated) {
            GameObject socketIO = Instantiate(socketIOPrefab);
            DontDestroyOnLoad(socketIO);
            socketIO.tag = "SocketIO";
            socket = socketIO.GetComponent<SocketIOComponent>();
            socketIOInstantiated = true;

            socket.On("connect", (SocketIOEvent e) => {
                Debug.Log("Connected to Node server!");
                AuthenticateServer();
            });

            socket.On("initGame", (SocketIOEvent e) => {
                if (!launched) {
                    StartServer();
                    launched = true;
                }
            });

            socket.On("endGame", (SocketIOEvent e) => {
                launched = false;
                SceneManager.LoadScene("ServerWaitGame");
            });
        }
        
        if (!launched) {
            SceneManager.LoadScene("ServerWaitGame");
        }
    }

    public void ConnectClientToServer(InputField serverIpField) {
        networkAddress = serverIpField.text;

        StartClient();
    }

    void AuthenticateServer() {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["name"] = "game";
        socket.Emit("authentication", new JSONObject(data));
    }
}
