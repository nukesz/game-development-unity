using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SocketIO;

public class Spawner : MonoBehaviour
{
    public GameObject myPlayer;
    public GameObject playerPrefab;
    public SocketIOComponent socket;

    private Dictionary<string, GameObject> _players = new Dictionary<string, GameObject>();

    public void AddPlayer(string id, GameObject player)
    {
        player.GetComponent<NetworkEntity>().id = id;
        _players.Add(id, player);
    }

    public GameObject SpawnPlayer(string id)
    {
        var player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        player.GetComponent<ClickFollow>().myPlayer = myPlayer;
        AddPlayer(id, player);
        return player;
    }

    public GameObject FindPlayer(string id)
    {
        return _players[id];
    }

    public void Remove(string id)
    {
        var player = _players[id];
        Destroy(player);
        _players.Remove(id);
    }


}