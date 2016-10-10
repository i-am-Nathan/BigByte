﻿using UnityEngine;
using System.Collections;

public class TrapDamage : MonoBehaviour {
    Transform player1;
    Transform player2;
    private bool _hitp1 = true;
    private bool _hitp2 = true;
    private float _nodamageTime = 3f;
    private float _p1HitTime = 0f;
    private float _p2HitTime = 0f;

    void Start()
    {
        player1 = GameObject.FindGameObjectWithTag("Player").transform;
        player2 = GameObject.FindGameObjectWithTag("Player2").transform;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player 1")
        {
            //if able to be hit p1
            if (_hitp1)
            {
                _hitp1 = false;
                _p1HitTime = _nodamageTime;
                Debug.Log("DAMAGE P1");
                player1.GetComponent<BaseEntity>().Damage(30, this.gameObject.transform);
            }
        } else if (other.name == "Player2")
        {
            if (_hitp2)
            {
                _hitp2 = false;
                _p2HitTime = _nodamageTime;
                Debug.Log("DAMAGE P2");
                player2.GetComponent<BaseEntity>().Damage(30, this.gameObject.transform);
            }

        }
    }

    void Update()
    {
        if (_p1HitTime == 0)
        {
            Debug.Log("p1 true");
            _hitp1 = true;
        } else
        {
            _p1HitTime -= Time.deltaTime;
            Debug.Log(_p1HitTime);
        }
        if (_p2HitTime == 0)
        {
            Debug.Log("p2 true");
            _hitp2 = true;
        } else
        {
            _p2HitTime -= Time.deltaTime;
            Debug.Log(_p2HitTime);
        }
    }
}
