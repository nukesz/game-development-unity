using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SocketIO;

public class Network : MonoBehaviour
{
    static SocketIOComponent socket;

    public GameObject myPlayer;
    public Spawner spawner;


    void Start()
    {
        socket = GetComponent<SocketIOComponent>();
        socket.On("open", OnConnected);
        socket.On("register", OnRegister);
        socket.On("spawn", OnSpawned);
        socket.On("move", OnMoved);
        socket.On("follow", OnFollow);
        socket.On("attack", OnAttack);
        socket.On("disconnected", OnDisconnected);
        socket.On("requestPosition", OnRequestPosition);
        socket.On("updatePosition", OnUpdatePosition);
    }

    private void OnConnected(SocketIOEvent e)
    {
        Debug.Log("connected");
    }

    private void OnRegister(SocketIOEvent e)
    {
        Debug.Log("Successfully registered with id: " + e.data);
        spawner.AddPlayer(e.data["id"].str, myPlayer);
    }

    private void OnSpawned(SocketIOEvent e)
    {
        Debug.Log("spawned " + e.data["id"].str);
        var player = spawner.SpawnPlayer(e.data["id"].str);
        if (e.data["x"])
        {
            var movePosition = GetVectorFromJson(e);
            var navigatePos = player.GetComponent<Navigator>();
            navigatePos.NavigateTo(movePosition);
        }
    }

    private void OnMoved(SocketIOEvent e)
    {
        Debug.Log("player is moving: " + e.data);
        var id = e.data["id"].str;
        var pos = GetVectorFromJson(e);
        var player = spawner.FindPlayer(id);
        var navigatePos = player.GetComponent<Navigator>();
        navigatePos.NavigateTo(pos);
    }

    private void OnFollow(SocketIOEvent e)
    {
        Debug.Log("follow request: " + e.data);
        var player = spawner.FindPlayer(e.data["id"].str);
        var targetTransform = spawner.FindPlayer(e.data["targetId"].str).transform;
        var target = player.GetComponent<Targeter>();
        target.target = targetTransform;
    }

    private void OnAttack(SocketIOEvent e)
    {
        Debug.Log("received attack: " + e.data);
        var targetPlayer = spawner.FindPlayer(e.data["targetId"].str);
        targetPlayer.GetComponent<Hittable>().health -= 10;
    }

    private void OnDisconnected(SocketIOEvent e)
    {
        var id = e.data["id"].str;
        Debug.Log("Player disconnected, id: " + id);
        spawner.Remove(id);
    }

    private void OnRequestPosition(SocketIOEvent e)
    {
        Debug.Log("server is asking position");
        socket.Emit("updatePosition", Network.VectorToJson(myPlayer.transform.position));
    }

    private void OnUpdatePosition(SocketIOEvent e)
    {
        Debug.Log("update position: " + e.data);
        var id = e.data["id"].str;
        var pos = GetVectorFromJson(e);
        var player = spawner.FindPlayer(id);
        player.transform.position = pos;
    }

    public static void Move(Vector3 position)
    {
        Debug.Log("sending position to node: " + VectorToJson(position));
        socket.Emit("move", VectorToJson(position));
    }

    public static void Follow(string id)
    {
        Debug.Log("sending follow player id: " + PlayerIdToJson(id));
        socket.Emit("follow", PlayerIdToJson(id));
    }

    public static void Attack(string targetId)
    {
        Debug.Log("attacking player: " + PlayerIdToJson(targetId));
        socket.Emit("attack", PlayerIdToJson(targetId));
    }


    private static Vector3 GetVectorFromJson(SocketIOEvent e)
    {
        return new Vector3(e.data["x"].n, 0, e.data["y"].n);
    }

    public static JSONObject VectorToJson(Vector3 vector)
    {
        var json = new JSONObject(JSONObject.Type.OBJECT);
        json.AddField("x", vector.x);
        json.AddField("y", vector.z);
        return json;
    }


    public static JSONObject PlayerIdToJson(string id)
    {
        var json = new JSONObject(JSONObject.Type.OBJECT);
        json.AddField("targetId", id);
        return json;
    }

}