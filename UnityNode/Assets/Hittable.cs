using UnityEngine;
using System.Collections;

public class Hittable : MonoBehaviour
{
    public float health = 100f;
    public float respawnTime = 5;

    public bool IsDead
    {
        get
        {
            return health <= 0;
        }
    }

    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void OnHit()
    {
        health -= 10;
        if (IsDead)
        {
            _animator.SetTrigger("Dead");
            Invoke("Spawn", respawnTime);
        }
    }

    void Spawn()
    {
        Debug.Log("Spawning my player");
        transform.position = Vector3.zero;
        health = 100;
        _animator.SetTrigger("Spawn");
    }
}