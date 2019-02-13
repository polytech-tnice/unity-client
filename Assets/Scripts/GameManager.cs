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

    public void CollisionDetected(TennisCourtZone.ZoneType type, int player) {
        int[] newScore = score.GetCurrentScore();
        pointInProgress = false;
        if (type == TennisCourtZone.ZoneType.IN) {
            if (++currentBallBounces >= 2) {
                newScore = score.IncrementScore(currentPlayer);
            }
        } else {
            newScore = score.IncrementScore(1-currentPlayer);
        }
        Debug.Log(newScore[0] + " " + newScore[1]);
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
}
