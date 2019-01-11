﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quobject.SocketIoClientDotNet.Client;
using Newtonsoft.Json;

public class DummySocketIO : MonoBehaviour
{
    struct Message {
        public string msg;
        public string device;
    }

    private Socket socket;

    void Destroy() {
        socket.Disconnect();
    }

    // Start is called before the first frame update
    void Start()
    {
        socket = IO.Socket("http://192.168.43.183:3000");
        socket.On (Socket.EVENT_CONNECT, () => {
            // Access to Unity UI is not allowed in a background thread, so let's put into a shared variable
            Debug.Log("Connected");
        });
        socket.On ("chat message", (data) => {
            string str = data.ToString();

            Debug.Log(str);
        });
        
    }

    // Update is called once per frame
    void Update()
    {
       if (Input.GetButtonDown("Fire1")) {
           socket.Emit("chat message", "coucou antoine steyer", "Iouniti");
       }
    }
}
