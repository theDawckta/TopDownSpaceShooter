﻿using UnityEngine;
using System.Collections;

public interface IEnemyState
{
    void UpdateState();

    void OnTriggerEnter(Collider other);

    void ToPatrolState();

    void ToEvadeState();

    void ToAttackState();

    void ToChaseState();
}