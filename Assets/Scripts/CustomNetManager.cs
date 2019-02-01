using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SocketIO;

public class CustomNetManager : NetworkManager
{
  [SerializeField]
  private GameObject socketIOPrefab;
  [SerializeField]
  private GameObject vrPrefab;

  [SerializeField]
  private GameObject cameraPrefab;

  private SocketIOComponent socket;
  private bool socketIOInstantiated = false;
  private bool launched = false;
  private int curPlayer = 0;

  public void InitGameServer()
  {
    if (!socketIOInstantiated)
    {
      GameObject socketIO = Instantiate(socketIOPrefab);
      DontDestroyOnLoad(socketIO);
      socketIO.tag = "SocketIO";
      socket = socketIO.GetComponent<SocketIOComponent>();
      socketIOInstantiated = true;

      socket.On("connect", (SocketIOEvent e) =>
      {
        Debug.Log("Connected to Node server!");
        AuthenticateServer();
      });

      socket.On("initGame", (SocketIOEvent e) =>
      {
        if (!launched)
        {
          StartServer();
          launched = true;
        }
      });

      socket.On("endGame", (SocketIOEvent e) =>
      {
        launched = false;
        SceneManager.LoadScene("ServerWaitGame");
      });
    }

    if (!launched)
    {
      SceneManager.LoadScene("ServerWaitGame");
    }
  }

  public void ConnectClientToServer(InputField serverIpField)
  {
    networkAddress = serverIpField.text;
    curPlayer = 0;
    StartClient();
  }

  void AuthenticateServer()
  {
    Dictionary<string, string> data = new Dictionary<string, string>();
    data["name"] = "game";
    socket.Emit("authentication", new JSONObject(data));
  }

  public void ConnectCameraToServer(InputField serverIpField)
  {
    networkAddress = serverIpField.text;
    curPlayer = 1;
    StartClient();
  }

  //Called on client when connect
  public override void OnClientConnect(NetworkConnection conn) {       

      // Create message to set the player
      IntegerMessage msg = new IntegerMessage(curPlayer);      

      // Call Add player and pass the message
      ClientScene.AddPlayer(conn, 0, msg);
  }

  public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader ) { 
    // Read client message and receive index
    if (extraMessageReader != null) {
        var stream = extraMessageReader.ReadMessage<IntegerMessage> ();
        curPlayer = stream.value;
    }

    if (curPlayer == 0) { // player
      var player = Instantiate(vrPrefab, GetStartPosition());
      NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    } else { // camera
      GameObject spawnPoint = GameObject.Find("Camera Spawn");
      var player = Instantiate(cameraPrefab, spawnPoint.transform);
      NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }
  }
}
