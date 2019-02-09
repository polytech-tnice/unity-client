using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Score score;

    [SerializeField]
    private SocketIOComponent socket;

    public int CurrentPlayer { get { return currentPlayer; }}
    private int currentPlayer;

    public bool PointInProgress { get { return pointInProgress; }}
    private bool pointInProgress;

    void Start() {
        pointInProgress = false;
        currentPlayer = 0;
    }

    public void CollisionDetected(TennisCourtZone.ZoneType type, int player) {
        int[] newScore;
        pointInProgress = false;
        if (type == TennisCourtZone.ZoneType.IN) {
            newScore = score.IncrementScore(currentPlayer);
        } else {
            newScore = score.IncrementScore(1-currentPlayer);
        }
        Debug.Log(newScore[0] + " " + newScore[1]);
    }

    public void Service(int player) {
        currentPlayer = player;
        pointInProgress = true;
    }
}
