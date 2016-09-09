using UnityEngine;
using System.Collections;

public class ClickFollow : MonoBehaviour, IClickable
{
    public GameObject myPlayer;
    public NetworkEntity networkEntity;

    private Targeter _myPlayerTargeter;

    void Start()
    {
        networkEntity = GetComponent<NetworkEntity>();
        _myPlayerTargeter = myPlayer.GetComponent<Targeter>();
    }

    public void OnClick(RaycastHit hit)
    {
        Debug.Log("following: " + hit.collider.gameObject.name);
        Network.Follow(networkEntity.id);
        _myPlayerTargeter.target = transform;
    }
}