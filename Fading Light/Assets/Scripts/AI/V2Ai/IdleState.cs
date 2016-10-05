using UnityEngine;
using System.Collections;
using System;

//State for monster being in idle
public class IdleState : IEnemyState
{
    private readonly StatePatternEnemy enemy;

    //constructor for idle state
    public IdleState (StatePatternEnemy statePatternEnemy)
    {
        this.enemy = statePatternEnemy;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Player2"))
        {

        }
    }

    public void ToAlertState()
    {
        enemy.currentState = enemy.alertState;
    }

    public void ToChaseState()
    {
        enemy.currentState = enemy.chaseState;
    }

    public void ToIdleState()
    {
        Debug.Log("CANNOT GO INTO OWN STATE");
    }

    public void UpdateState()
    {
        Roam();
        Idle();
    }

    private void Roam()
    {

    }

    private void Idle()
    {

    }
}
