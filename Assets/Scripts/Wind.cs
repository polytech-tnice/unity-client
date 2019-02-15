using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{

    public float windSpeed;

    public Vector3 windDirection;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Ball");
        foreach (GameObject go in gos)
        {
            MeshArea area = go.GetComponent<MeshArea>();
            Rigidbody rb = go.GetComponent<Rigidbody>();
            float speed = windSpeed / 3.6f;
            float surface = area != null ? area.Area : 1.0f;
            rb.AddForce((0.5f * Mathf.Pow(speed, 2) * surface / 2) * windDirection.normalized);
        }
    }
}
