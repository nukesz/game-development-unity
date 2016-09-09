using UnityEngine;
using System.Collections;

public class Navigator : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Targeter _targeter;


    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _targeter = GetComponent<Targeter>();
    }

    void Update()
    {
        GetComponent<Animator>().SetFloat("Distance", _agent.remainingDistance);
    }

    public void NavigateTo(Vector3 position)
    {
        _agent.SetDestination(position);
        _targeter.target = null;
    }
}