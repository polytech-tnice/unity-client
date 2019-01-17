using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class DummySocketIO : MonoBehaviour
{
    [SerializeField]
    private GameObject socketGo;

    private SocketIOComponent socket;

    // Start is called before the first frame update
    void Start() {
        socket = socketGo.GetComponent<SocketIOComponent>();
        socket.On("open", TestOpen);
		socket.On("error", TestError);
		socket.On("close", TestClose);
        socket.On("chat message", (SocketIOEvent e) => {
            Debug.Log(e.name + ": " + e.data);
        });

        socket.On("connected", (SocketIOEvent e) => {
            Debug.Log("I'm connected!");
        });
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    public void TestOpen(SocketIOEvent e)
	{
		Debug.Log("[SocketIO] Open received: " + e.name + " " + e.data);
	}

	public void TestError(SocketIOEvent e)
	{
		Debug.Log("[SocketIO] Error received: " + e.name + " " + e.data);
	}
	
	public void TestClose(SocketIOEvent e)
	{	
		Debug.Log("[SocketIO] Close received: " + e.name + " " + e.data);
	}
}
