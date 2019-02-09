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

    private int currentPlayer;

    void Start() {
        currentPlayer = 0;
    }

    public void CollisionDetected(TennisCourtZone.ZoneType type, int player) {
        int[] newScore;
        if (type == TennisCourtZone.ZoneType.IN) {
            newScore = score.IncrementScore(player);
        } else {
            newScore = score.IncrementScore(1-player);
        }
        Debug.Log(newScore[0] + " " + newScore[1]);
    }
}
