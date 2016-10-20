// file:	Assets\Scripts\Player\Player.cs
//
// summary:	Implements the player class

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>   Superclass for players. </summary>
///
/// <remarks>    . </remarks>

public class Player: BaseEntity
{
    /// <summary>   True if this object is attacking. </summary>
    private bool _isAttacking;

    /// <summary>   True to debug. </summary>
    private bool DEBUG = true;

    /// <summary>   True if this object can take damage. </summary>
    public bool CanTakeDamage = true;

    /// <summary>   The drink potion. </summary>
    public AudioClip DrinkPotion;
    /// <summary>   Source for the. </summary>
    private AudioSource _source;

    /// <summary>   Starts this object. </summary>
    ///
 

    protected override void Start()
    {
        base.Start();
        try
        {
            _source = GetComponent<AudioSource>();
            this.transform.FindChild("AttackParticles").gameObject.SetActive(false);
            this.transform.FindChild("DefenseParticles").gameObject.SetActive(false);
        }
        catch { }
    }

    /// <summary>   Determines whether this instance is attacking. </summary>
    ///
 
    ///
    /// <returns>   <c>true</c> if this instance is attacking; otherwise, <c>false</c>. </returns>

    public bool isAttacking()
    {
        return _isAttacking;
    }

    /// <summary>   Sets the attacking. </summary>
    ///
 
    ///
    /// <param name="a">    if set to <c>true</c> [a]. </param>

    public void setAttacking(bool a)
    {
        _isAttacking = a;
    }

    /// <summary>   True to attack pot active. </summary>
    private bool _attackPotActive = false;
    /// <summary>   True to defense pot active. </summary>
    private bool _defensePotActive = false;
    /// <summary>   True to health pot active. </summary>
	private bool _healthPotActive = false;

    /// <summary>   The attack pot time left. </summary>
    private float _attackPotTimeLeft;
    /// <summary>   The defense pot time left. </summary>
    private float _defensePotTimeLeft;
    /// <summary>   Duration of the attack pot. </summary>
    private float _attackPotDuration = 3f;
    /// <summary>   Duration of the defense pot. </summary>
    private float _defensePotDuration = 3f;

    /// <summary>   Update the timers on certain effects the character is under. </summary>
    ///
 

    public void UpdateEffects ()
    {
        if (_attackPotActive)
        {
            _attackPotTimeLeft -= Time.deltaTime;
            //text.text = "Time Left:" + Mathf.Round(timeLeft);
            if (_attackPotTimeLeft <= 0)
            {
                _attackPotActive = false;
				this.transform.FindChild ("AttackParticles").gameObject.SetActive(false);
            }
        }
        if (_defensePotActive)
        {
            _defensePotTimeLeft -= Time.deltaTime;
            //text.text = "Time Left:" + Mathf.Round(timeLeft);
            if (_defensePotTimeLeft <= 0)
            {
                _defensePotActive = false;
				this.transform.FindChild ("DefenseParticles").gameObject.SetActive(false);
            }
        }
    }

    /// <summary>   Queries if the attack pot is active. </summary>
    ///
 
    ///
    /// <returns>   True if the attack pot is active, false if not. </returns>

    public bool isAttackPotActive()
    {
        return _attackPotActive;
    }

    /// <summary>   Queries if the defense pot is active. </summary>
    ///
 
    ///
    /// <returns>   True if the defense pot is active, false if not. </returns>

	public bool isDefensePotActive()
	{
		return _defensePotActive;
	}

    /// <summary>   Queries if the health pot is active. </summary>
    ///
 
    ///
    /// <returns>   True if the health pot is active, false if not. </returns>

	public bool isHealthPotActive()
	{
		return _healthPotActive;
	}

    /// <summary>   Sets health pot active. </summary>
    ///
 

	public void SetHealthPotActive ()
	{
		_healthPotActive = false;
	}

    /// <summary>   Health pot activated. </summary>
    ///
 

    public void HealthPotActivated()
    {
        //BEGIN THE BLOODY ANIM
        if (DEBUG) Debug.Log("Health pot activated");
        _source.PlayOneShot(DrinkPotion);
		_healthPotActive = true;
        if ((CurrentHealth + 30) > IntialHealth)
        {
            CurrentHealth = IntialHealth;
        }
        else
        {
            this.CurrentHealth = CurrentHealth + 30;
        }
    }

    /// <summary>   Attack pot activated. </summary>
    ///
 

    public void AttackPotActivated()
    {
        //BEGIN THE BLOODY ANIM
        if (DEBUG) Debug.Log("Attack pot activated");
        _source.PlayOneShot(DrinkPotion);
		this.transform.Find ("AttackParticles").gameObject.SetActive(true);
		_attackPotTimeLeft = _attackPotDuration;
		_attackPotActive = true;
    }

    /// <summary>   Defense pot activated. </summary>
    ///
 

    public void DefensePotActivated()
    {
        if (DEBUG) Debug.Log("Defense pot activated");
        _source.PlayOneShot(DrinkPotion);
		this.transform.Find ("DefenseParticles").gameObject.SetActive(true);

		_defensePotTimeLeft = _defensePotDuration;
		_defensePotActive = true;
    }
}
