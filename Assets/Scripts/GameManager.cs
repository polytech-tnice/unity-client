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

  public bool PointInProgress { get { return pointInProgress; } }
  [SyncVar]
  private bool pointInProgress;

  private int pointCounter;

  public bool ReadyToPlay { get {
    return nextIdToCreate >= 2;
  }}

  [SyncVar]
  private int nextIdToCreate = 0;

  void Start()
  {
    pointInProgress = false;
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

  public void Service(int player)
  {
    if (isServer)
    {
      Debug.Log("Service of player : " + player);
      readyForPoint = false;
      currentPlayer = player;
      pointInProgress = true;
      currentBallBounces = 0;
    }
  }

  public int CreateNewId()
  {
    int res = nextIdToCreate;
    if (isServer)
    {
      nextIdToCreate++;
    }
    return res;
  }

  void FinishPoint(int winnerId, Ball ball)
  {
    if (isServer)
    {
      pointInProgress = false;
      int[] newScore = score.GetCurrentScore();
      newScore = score.IncrementScore(winnerId);
      ball.IsInGame = false;
      Debug.Log("Score : " + newScore[0] + " - " + newScore[1]);

      currentPlayer = startingPlayer = 1 - startingPlayer;
    }

  }
}
