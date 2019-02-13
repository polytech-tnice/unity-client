using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SocketIO;

public class GameManager : NetworkBehaviour
{
    [SerializeField]
    private Score score;

    [SerializeField]
    private SocketIOComponent socket;

    public int CurrentPlayer { get { return currentPlayer; }}
    [SyncVar]
    private int currentPlayer;
    
    private int currentBallBounces;


    public bool PointInProgress { get { return pointInProgress; }}
    [SyncVar]
    private bool pointInProgress;

    [SyncVar]
    private int nextIdToCreate = 0;

    void Start() {
        pointInProgress = false;
        currentPlayer = 0;
    }

    public void CollisionDetected(TennisCourtZone.ZoneType type, int owner, Ball ball) {
        if (type == TennisCourtZone.ZoneType.IN) {
            if (++currentBallBounces >= 2) {
                if (owner == currentPlayer) {
                    FinishPoint(1 - currentPlayer, ball);
                } else {
                    FinishPoint(currentPlayer, ball);
                }
            }
        } else {
            FinishPoint(1 - currentPlayer, ball);
        }
    }

    public void Service(int player) {
        if (isServer) {
            Debug.Log("Service of player : " + player);
            currentPlayer = player;
            pointInProgress = true;
            currentBallBounces = 0;
        }
    }

    public int CreateNewId() {
        int res = nextIdToCreate;
        if (isServer) {
            nextIdToCreate++;
        }
        return res;
    }

    void FinishPoint(int winnerId, Ball ball) {
        pointInProgress = false;
        int[] newScore = score.GetCurrentScore();
        newScore = score.IncrementScore(winnerId);
        ball.IsInGame = false;
        Debug.Log("Score : " + newScore[0] + " - " + newScore[1]);
    }
}
