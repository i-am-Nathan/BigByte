using UnityEngine;
using System.Collections;

public interface IEnemyState {

    void UpdateState();
    void OnTriggerEnter(Collider other);
    void ToIdleState();
    void ToChaseState();
    void ToAlertState();
    void ToAttackState();

}
