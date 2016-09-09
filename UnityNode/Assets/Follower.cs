using UnityEngine;
using System.Collections;

public class Follower : MonoBehaviour
{
    public Transform target;

    public float scanFrequency = 0.5f;
    public float stopFollowDistance = 2f;

    private float lastScanTime = 0f;

    private NavMeshAgent _agent;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (IsReadyToScan() && !IsInRange())
        {
            Debug.Log("scanning nav path");
            lastScanTime = Time.time;
            _agent.SetDestination(target.position);
        }
    }

    private bool IsInRange()
    {
        var distance = Vector3.Distance(target.position, transform.position);
        return distance < stopFollowDistance;
    }

    private bool IsReadyToScan()
    {
        return target && Time.time - lastScanTime > scanFrequency;
    }
}