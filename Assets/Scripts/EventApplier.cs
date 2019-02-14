using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class EventApplier : MonoBehaviour
{
    [SerializeField]
    private Wind wind;

    private SocketIOComponent socket;

    private GameObject[] windables;
    // Start is called before the first frame update
    void Start()
    {
        GameObject go = GameObject.FindGameObjectWithTag("SocketIO");
        socket = go.GetComponent<SocketIOComponent>();

        socket.On("actionEvent", (SocketIOEvent e) => {
            Debug.Log(e.data);
            Dictionary<string, string> data = e.data.ToDictionary();

            if (data.ContainsKey("speed")) {
                wind.windSpeed = int.Parse(data["speed"]);

                switch(data["direction"]) {
                    case "Nord":
                        wind.windDirection = new Vector3(0, 0, 1.0f);
                        break;
                    case "Sud":
                        wind.windDirection = new Vector3(0, 0, -1.0f);
                        break;
                    case "Est":
                        wind.windDirection = new Vector3(1.0f, 0, 0);
                        break;
                    case "Ouest":
                        wind.windDirection = new Vector3(-1.0f, 0, 0);
                        break;
                }
            }

        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
