using UnityEngine;
using System.Collections;
using System;

public class AttackState : IEnemyState
{
    private readonly StatePatternEnemy enemy;

    //constructor for idle state
    public AttackState(StatePatternEnemy statePatternEnemy)
    {
        this.enemy = statePatternEnemy;
    }

    public void OnTriggerEnter(Collider other)
    {
        throw new NotImplementedException();
    }

    public void ToAlertState()
    {
        throw new NotImplementedException();
    }

    public void ToAttackState()
    {
        Debug.Log("CANNOT GO INTO OWN STATE");
    }

    public void ToChaseState()
    {
        throw new NotImplementedException();
    }

    public void ToIdleState()
    {
        throw new NotImplementedException();
    }

    public void UpdateState()
    {
        throw new NotImplementedException();
    }
}
