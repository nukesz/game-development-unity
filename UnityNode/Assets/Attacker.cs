﻿using System;
using UnityEngine;
using System.Collections;
using JetBrains.Annotations;

public class Attacker : MonoBehaviour
{
    public float AttackDistance = 1f;
    public float AttackRate = 2f;

    private float _lastAttackTime = 0f;

    private Targeter _targeter;

    void Start()
    {
        _targeter = GetComponent<Targeter>();
    }

    void Update()
    {
        if (!IsReadyToAttack())
        {
            return;
        }

        if (IsTargetDead())
        {
            _targeter.ResetTarget();
            return;
        }
        if (_targeter.IsInRange(AttackDistance))
        {
            Debug.Log("Attacking");
            var targetId = _targeter.target.GetComponent<NetworkEntity>().id;
            Network.Attack(targetId);
            _lastAttackTime = Time.time;
        }
    }

    bool IsReadyToAttack()
    {
        return _targeter.target && Time.time - _lastAttackTime > AttackRate;
    }

    bool IsTargetDead()
    {
        return _targeter.target.GetComponent<Hittable>().IsDead;
    }

}