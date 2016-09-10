using UnityEngine;
using System.Collections;

public class Navigator : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Targeter _targeter;
    private Animator _animator;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _targeter = GetComponent<Targeter>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        _animator.SetFloat("Distance", _agent.remainingDistance);
    }

    public void NavigateTo(Vector3 position)
    {
        _agent.SetDestination(position);
        _targeter.target = null;
        _animator.SetBool("Attack", false);

    }
}