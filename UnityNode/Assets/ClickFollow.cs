using UnityEngine;
using System.Collections;

public class ClickFollow : MonoBehaviour, IClickable {

    public Follower myPlayerFollower;

    public void OnClick(RaycastHit hit)
    {
        Debug.Log("following: " + hit.collider.gameObject.name);
        myPlayerFollower.target = transform;
    }
}
