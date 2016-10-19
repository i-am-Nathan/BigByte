using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Superclass for players
/// </summary>
public class Player: BaseEntity
{
    private bool _isAttacking;

    private bool DEBUG = true;


    public AudioClip DrinkPotion;
    private AudioSource _source;
    
    protected override void Start()
    {
        base.Start();
        _source = GetComponent<AudioSource>();
		this.transform.FindChild ("AttackParticles").gameObject.SetActive(false);
		this.transform.FindChild ("DefenseParticles").gameObject.SetActive(false);
    }

    /// <summary>
    /// Determines whether this instance is attacking.
    /// </summary>
    /// <returns>
    ///   <c>true</c> if this instance is attacking; otherwise, <c>false</c>.
    /// </returns>
    public bool isAttacking()
    {
        return _isAttacking;
    }

    /// <summary>
    /// Sets the attacking.
    /// </summary>
    /// <param name="a">if set to <c>true</c> [a].</param>
    public void setAttacking(bool a)
    {
        _isAttacking = a;
    }

    private bool _attackPotActive = false;
    private bool _defensePotActive = false;
	private bool _healthPotActive = false;

    private float _attackPotTimeLeft;
    private float _defensePotTimeLeft;
    private float _attackPotDuration = 10f;
    private float _defensePotDuration = 10f;
    
    /// <summary>
    /// Update the timers on certain effects the character is under
    /// </summary>
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

    public bool isAttackPotActive()
    {
        return _attackPotActive;
    }

	public bool isDefensePotActive()
	{
		return _defensePotActive;
	}

	public bool isHealthPotActive()
	{
		return _healthPotActive;
	}

	public void SetHealthPotActive ()
	{
		_healthPotActive = false;
	}

    /// <summary>
    /// 
    /// </summary>
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

    /// <summary>
    /// 
    /// </summary>
    public void AttackPotActivated()
    {
        //BEGIN THE BLOODY ANIM
        if (DEBUG) Debug.Log("Attack pot activated");
        _source.PlayOneShot(DrinkPotion);
		this.transform.Find ("AttackParticles").gameObject.SetActive(true);
		_attackPotTimeLeft = _attackPotDuration;
		_attackPotActive = true;
    }

    /// <summary>
    /// 
    /// </summary>
    public void DefensePotActivated()
    {
        if (DEBUG) Debug.Log("Defense pot activated");
        _source.PlayOneShot(DrinkPotion);
		this.transform.Find ("DefenseParticles").gameObject.SetActive(true);

		_defensePotTimeLeft = _defensePotDuration;
		_defensePotActive = true;
    }
}
