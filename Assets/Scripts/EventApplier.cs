using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class EventApplier : MonoBehaviour
{
    private SocketIOComponent socket;
    // Start is called before the first frame update
    void Start()
    {
        GameObject go = GameObject.FindGameObjectWithTag("SocketIO");
        socket = go.GetComponent<SocketIOComponent>();

        socket.On("actionEvent", (SocketIOEvent e) => {
            Debug.Log(e.data.ToString(true)); 
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
