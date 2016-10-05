using UnityEngine;
using System.Collections;

public interface IEnemyState {

    void UpdateState();
    void OnTriggerENter(Collider other);
    void ToIdleState();
    void ToChaseState();
    void ToAlertState();

}
