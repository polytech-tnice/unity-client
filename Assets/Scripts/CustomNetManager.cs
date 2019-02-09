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
  private GameObject controllerPrefab;

  [SerializeField]
  private GameObject cameraPrefab;

  private SocketIOComponent socket;
  private bool socketIOInstantiated = false;
  private bool launched = false;
  private int curPlayer = 0;
  private string gameName;

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
        initGameInBackEnd();

        // TODO REMOVE THIS LINE
        launchGame();
      });

      socket.On("gameLaunched", (SocketIOEvent e) =>
      {
        if (!launched)
        {
          StartServer();
          NetworkServer.RegisterHandler(1002, OnBallService);
          gameName = e.data.ToDictionary()["name"];
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

  void initGameInBackEnd()
  {
    Dictionary<string, string> data = new Dictionary<string, string>();
    data["game_name"] = "Game1";
    data["player1_name"] = "Joueur1";
    data["player2_name"] = "Joueur2";
    socket.Emit("initGame", new JSONObject(data));
  }

  void launchGame()
  {
    Dictionary<string, string> data = new Dictionary<string, string>();
    data["name"] = "Game1";
    socket.Emit("launchGame", new JSONObject(data));
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

  public void ConnectControllerToServer(InputField serverIpField)
  {
    networkAddress = serverIpField.text;
    curPlayer = 2;
    StartClient();
  }

  //Called on client when connect
  public override void OnClientConnect(NetworkConnection conn)
  {

    // Create message to set the player
    IntegerMessage msg = new IntegerMessage(curPlayer);

    // Call Add player and pass the message
    ClientScene.AddPlayer(conn, 0, msg);
  }

  public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
  {
    // Read client message and receive index
    if (extraMessageReader != null)
    {
      var stream = extraMessageReader.ReadMessage<IntegerMessage>();
      curPlayer = stream.value;
    }

    if (curPlayer == 0)
    { // player
      var player = Instantiate(vrPrefab, GetStartPosition());
      NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }
    if (curPlayer == 1)
    { // camera
      GameObject spawnPoint = GameObject.Find("Camera Spawn");
      var player = Instantiate(cameraPrefab, spawnPoint.transform);
      NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }
    if (curPlayer == 2)
    {
      var player = Instantiate(controllerPrefab, GetStartPosition());
      NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }
  }

  void OnBallService(NetworkMessage netMsg)
  {
    StringMessage message = netMsg.ReadMessage<StringMessage>();

    Dictionary<string, string> data = new Dictionary<string, string>();
    data["game_name"] = gameName;

    JSONObject json = new JSONObject(data);
    json.SetField("player1_score", 0);
    json.SetField("player2_score", 0);

    socket.Emit("updateScore", json);
  }
}
