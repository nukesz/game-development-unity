using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    public GameObject myPlayer;
    public GameObject playerPrefab;

    private Dictionary<string, GameObject> _players = new Dictionary<string, GameObject>();


    public GameObject SpawnPlayer(string id)
    {
        var player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        player.GetComponent<ClickFollow>().myPlayerFollower = myPlayer.GetComponent<Follower>();
        _players.Add(id, player);
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