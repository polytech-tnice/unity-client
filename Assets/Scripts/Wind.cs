using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{

    [SerializeField]
    private float windSpeed;

    [SerializeField]
    private Vector3 windDirection;

    [SerializeField]
    private Rigidbody[] rigidbodies;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach (Rigidbody rb in rigidbodies)
        {
            MeshArea area = rb.GetComponent<MeshArea>();
            float speed = windSpeed / 3.6f;
            float surface = area != null ? area.Area : 1.0f;
            rb.AddForce((0.5f * Mathf.Pow(speed, 2) * surface / 2) * windDirection.normalized);
        }
    }
}
