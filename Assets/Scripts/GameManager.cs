using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SocketIO;

public class GameManager : NetworkBehaviour
{
  [SerializeField]
  private Score score;

  private SocketIOComponent socket;

  public int CurrentPlayer { get { return currentPlayer; } }
  [SyncVar]
  private int currentPlayer;
  private int startingPlayer;

  private int currentBallBounces;

  public bool ReadyForPoint { get { return readyForPoint; } }
  [SyncVar]
  private bool readyForPoint;
  private int pointCounter;

  public bool ReadyToPlay { get {
    return nextIdToCreate >= 2;
  }}

  [SyncVar]
  private int nextIdToCreate = 0;

  public string GameName {get; set;}

  void Start()
  {
    currentPlayer = startingPlayer = 0;
    readyForPoint = true;

    if (isServer)
    {
      socket = GameObject.Find("SocketIO TNice(Clone)").GetComponent<SocketIOComponent>();

      socket.On("connect", (SocketIOEvent e) =>
      {
        Debug.Log("Connected on game manager!");
      });

      socket.On("playPoint", (SocketIOEvent e) =>
      {
        Debug.Log("Ready for point");
        this.readyForPoint = true;
      });
    }
  }

  public void CollisionDetected(TennisCourtZone.ZoneType type, int owner, Ball ball)
  {
    if (type == TennisCourtZone.ZoneType.IN)
    {
      if (++currentBallBounces >= 2)
      {
        if (owner == currentPlayer)
        {
          FinishPoint(1 - currentPlayer, ball);
        }
        else
        {
          FinishPoint(currentPlayer, ball);
        }
      }
    }
    else
    {
      FinishPoint(1 - currentPlayer, ball);
    }
  }

  public void SetCurrentPlayer(int player)
  {
    currentPlayer = player;
  }

  public void StartPoint() {
    if (isServer) {
      readyForPoint = false;
      currentPlayer = startingPlayer = 1 - startingPlayer;

      // Update score on node server
      Dictionary<string, string> data = new Dictionary<string, string>();
      data["game_name"] = GameName;

      JSONObject json = new JSONObject(data);
      int[] currentScore = score.GetCurrentScore();
      json.SetField("player1_score", currentScore[0]);
      json.SetField("player2_score", currentScore[1]);

      socket.Emit("updateScore", json);
      Debug.Log("Send update score");
    }
  }

  public void Service(int player)
  {
    if (isServer)
    {
      Debug.Log("Service of player : " + player);
      readyForPoint = false; // wait for effects to be decided
      currentBallBounces = 0;
    }
  }

  public int CreateNewId()
  {
    int res = nextIdToCreate;
    if (isServer)
    {
      nextIdToCreate++;
      if (nextIdToCreate == 2) {
        StartPoint();
      }
    }
    return res;
  }

  void FinishPoint(int winnerId, Ball ball)
  {
    if (isServer)
    {
      int[] newScore = score.GetCurrentScore();
      newScore = score.IncrementScore(winnerId);
      ball.IsInGame = false;
      Debug.Log("Score : " + newScore[0] + " - " + newScore[1]);

      StartPoint();
    }
  }
}
