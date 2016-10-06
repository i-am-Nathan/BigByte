using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

public class PlayerDamageTests : MonoBehaviour
{
    [Test]
    public void PlayerTest_Damaged()
    {
        // Arrange
        //var playerToDamage = gameObject.AddComponent<PlayerController>();
        var playerToDamage = new PlayerController();
        playerToDamage.MockUp();

        // Act
        playerToDamage.Damage(2.0f, null);

        // Assert
        Assert.That(playerToDamage.CurrentHealth, Is.EqualTo(48));
        Assert.That(playerToDamage.isDead, Is.EqualTo(false));
    }

    [Test]
    public void PlayerTest_Killed()
    {
        // Arrange
        //var playerToDamage = gameObject.AddComponent<PlayerController>();
        var playerToDamage = new PlayerController();
        playerToDamage.MockUp();

        // Act
        playerToDamage.Damage(51.0f, null);

        // Assert
        Assert.That(playerToDamage.isDead, Is.EqualTo(true));
    }
}
