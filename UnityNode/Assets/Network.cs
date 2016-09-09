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
            var x = GetFloatFromJson(e.data, "x");
            var y = GetFloatFromJson(e.data, "y");
            var movePosition = new Vector3(x, 0, y);
            var navigatePos = player.GetComponent<Navigator>();
            navigatePos.NavigateTo(movePosition);
        }
    }

    private void OnMoved(SocketIOEvent e)
    {
        Debug.Log("player is moving: " + e.data);
        var id = e.data["id"].str;
        var x = GetFloatFromJson(e.data, "x");
        var y = GetFloatFromJson(e.data, "y");
        var pos = new Vector3(x, 0, y);

        var player = spawner.FindPlayer(id);
        var navigatePos = player.GetComponent<Navigator>();
        navigatePos.NavigateTo(pos);
    }

    private void OnFollow(SocketIOEvent e)
    {
        Debug.Log("follow request: " + e.data);
        var player = spawner.FindPlayer(e.data["id"].str);
        var target = spawner.FindPlayer(e.data["targetId"].str);
        var follower = player.GetComponent<Follower>();
        follower.target = target.transform;

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
        socket.Emit("updatePosition", new JSONObject(Network.VectorToJson(myPlayer.transform.position)));
    }

    private void OnUpdatePosition(SocketIOEvent e)
    {
        Debug.Log("update position: " + e.data);
        var id = e.data["id"].str;
        var x = GetFloatFromJson(e.data, "x");
        var y = GetFloatFromJson(e.data, "y");
        var pos = new Vector3(x, 0, y);

        var player = spawner.FindPlayer(id);
        player.transform.position = pos;
    }


    private float GetFloatFromJson(JSONObject json, string key)
    {
        return float.Parse(json[key].str);
    }

    public static string VectorToJson(Vector3 vector)
    {
        return string.Format(@"{{""x"":""{0}"", ""y"":""{1}""}}", vector.x, vector.z);
    }

    public static string PlayerIdToJson(string id)
    {
        return string.Format(@"{{""targetId"":""{0}""}}", id);
    }
}