using UnityEngine;
using System.Collections;

public class Navigator : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Follower _follower;


    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _follower = GetComponent<Follower>();
    }

    void Update()
    {
        GetComponent<Animator>().SetFloat("Distance", _agent.remainingDistance);
    }

    public void NavigateTo(Vector3 position)
    {
        _agent.SetDestination(position);
        _follower.target = null;
    }
}