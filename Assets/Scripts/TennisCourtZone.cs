using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TennisCourtZone : MonoBehaviour
{
    public enum ZoneType {
        IN,
        OUT
    }

    [Range(0,1)]
    [SerializeField]
    private int player;

    [SerializeField]
    private ZoneType type;

    void OnTriggerEnter(Collider col) {
        Debug.Log(type.ToString() + ' ' + player);
    }

}
