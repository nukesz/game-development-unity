using UnityEngine;
using System.Collections;

public class Targeter : MonoBehaviour
{

    public Transform target;


    public bool IsInRange(float stopFollowDistance)
    {
        var distance = Vector3.Distance(target.position, ((Component) this).transform.position);
        return distance < stopFollowDistance;
    }

    public void ResetTarget()
    {
        target = null;
    }
}