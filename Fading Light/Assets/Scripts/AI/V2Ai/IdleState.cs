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

    //On trigger collision with another player
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
        Observe();
        Idle();
    }

    //observes to find the player
    private void Observe()
    {
        RaycastHit hit;
        if (Physics.Raycast(enemy.eyes.transform.position, enemy.eyes.transform.forward, out hit, enemy.HardActivationDistance) && (hit.collider.CompareTag("Player") || hit.collider.CompareTag("Player2")))
        {
            enemy.playerTarget = hit.transform;
            ToChaseState();
        }

    }

    private void Idle()
    {

    }

    public void ToAttackState()
    {
        throw new NotImplementedException();
    }
}
