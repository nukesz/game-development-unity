using UnityEngine;
using System.Collections;

public class Follower : MonoBehaviour
{
    public Targeter Targeter;

    public float scanFrequency = 0.5f;
    public float stopFollowDistance = 2f;

    private float lastScanTime = 0f;

    private NavMeshAgent _agent;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        Targeter = GetComponent<Targeter>();
    }

    void Update()
    {
        if (IsReadyToScan() && !Targeter.IsInRange(stopFollowDistance))
        {
            Debug.Log("scanning nav path");
            lastScanTime = Time.time;
            _agent.SetDestination(Targeter.target.position);
        }
    }

    private bool IsReadyToScan()
    {
        return Targeter.target && Time.time - lastScanTime > scanFrequency;
    }
}