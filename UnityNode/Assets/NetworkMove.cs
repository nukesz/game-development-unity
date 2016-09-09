using System;
using UnityEngine;
using System.Collections;
using SocketIO;

public class NetworkMove : MonoBehaviour
{
    public SocketIOComponent socket;

    public void Move(Vector3 position)
    {
        Debug.Log("sending position to node: " + Network.VectorToJson(position));
        socket.Emit("move", new JSONObject(Network.VectorToJson(position)));
    }
}