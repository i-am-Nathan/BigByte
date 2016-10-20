// file:	Assets\Scripts\TrapScripts\TrapDamage.cs
//
// summary:	Implements the trap damage class

using UnityEngine;
using System.Collections;

/// <summary>   A trap damage. </summary>
///
/// <remarks>    . </remarks>

public class TrapDamage : MonoBehaviour {
    /// <summary>   The first player. </summary>
    Transform player1;
    /// <summary>   The second player. </summary>
    Transform player2;
    /// <summary>   True to hitp 1. </summary>
    private bool _hitp1 = true;
    /// <summary>   True to hitp 2. </summary>
    private bool _hitp2 = true;
    /// <summary>   The time till next hit. </summary>
    public float timeTillNextHit = 1f;
    /// <summary>   The 1 hit time. </summary>
    private float _p1HitTime = 0f;
    /// <summary>   The 2 hit time. </summary>
    private float _p2HitTime = 0f;
    /// <summary>   The damage. </summary>
    public float damage = 30f;

    /// <summary>   Starts this object. </summary>
    ///
 

    void Start()
    {
        player1 = GameObject.FindGameObjectWithTag("Player").transform;
        player2 = GameObject.FindGameObjectWithTag("Player2").transform;
    }

    /// <summary>   Executes the trigger enter action. </summary>
    ///
 
    ///
    /// <param name="other">    The other. </param>

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player 1")
        {
            //if able to be hit p1
            if (_hitp1)
            {
                _hitp1 = false;
                _p1HitTime = timeTillNextHit;
                player1.GetComponent<BaseEntity>().Damage(damage, this.gameObject.transform);
            }
        } else if (other.name == "Player2")
        {
            if (_hitp2)
            {
                _hitp2 = false;
                _p2HitTime = timeTillNextHit;
                player2.GetComponent<BaseEntity>().Damage(damage, this.gameObject.transform);
            }

        }
    }

    /// <summary>   Updates this object. </summary>
    ///
 

    void Update()
    {
        if (_p1HitTime < 0)
        {
            _hitp1 = true;
        } else
        {
            _p1HitTime -= Time.deltaTime;
        }
        if (_p2HitTime < 0)
        {
            _hitp2 = true;
        } else
        {
            _p2HitTime -= Time.deltaTime;
        }
    }
}
