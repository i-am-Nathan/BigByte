using UnityEngine;
using System.Collections;
using System;

public class AlertState : IEnemyState
{
    private readonly StatePatternEnemy enemy;

    //constructor for idle state
    public AlertState(StatePatternEnemy statePatternEnemy)
    {
        this.enemy = statePatternEnemy;
    }

    public void OnTriggerEnter(Collider other)
    {
        throw new NotImplementedException();
    }

    public void ToAlertState()
    {
        Debug.Log("CANNOT GO INTO OWN STATE");
    }

    public void ToAttackState()
    {
        throw new NotImplementedException();
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

    private void Observe()
    {
        RaycastHit hit;
        if (Physics.Raycast(enemy.eyes.transform.position, enemy.eyes.transform.forward, out hit, enemy.HardActivationDistance) && (hit.collider.CompareTag("Player") || hit.collider.CompareTag("Player2")))
        {
            enemy.playerTarget = hit.transform;
            ToChaseState();
        }

    }

}
