using UnityEngine;
using System.Collections;

public class Navigator : MonoBehaviour
{

    private NavMeshAgent _agent;

	void Awake ()
	{
	    _agent = GetComponent<NavMeshAgent>();
	}

    void Update()
    {
        GetComponent<Animator>().SetFloat("Distance", _agent.remainingDistance);
    }

	public void NavigateTo(Vector3 position)
	{
	    _agent.SetDestination(position);
	}
}
