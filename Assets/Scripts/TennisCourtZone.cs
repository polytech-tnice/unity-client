using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TennisCourtZone : MonoBehaviour
{
    public enum ZoneType {
        IN,
        OUT
    }

    [SerializeField]
    private GameManager gameManager;

    [Range(0,1)]
    [SerializeField]
    private int player;

    [SerializeField]
    private ZoneType type;

    void OnTriggerEnter(Collider col) {
        if (col.CompareTag("Ball")) {
            gameManager.CollisionDetected(type, player);
        }
    }

}
