// file:	Assets\Scripts\Tests\Editor\PlayerDamageTests.cs
//
// summary:	Implements the player damage tests class

using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

/// <summary>   A player damage tests. </summary>
///
/// <remarks>    . </remarks>

public class PlayerDamageTests : MonoBehaviour
{
    /// <summary>   (Unit Test Method) player test damaged. </summary>
    ///
 

    [Test]
    public void PlayerTest_Damaged()
    {
        // Arrange
        var playerToDamage = new PlayerController();
        playerToDamage.MockUp();

        // Act
        playerToDamage.Damage(2.0f, null);

        // Assert
        Assert.That(playerToDamage.CurrentHealth, Is.EqualTo(48));
        Assert.That(playerToDamage.isDead, Is.EqualTo(false));
    }

    /// <summary>   (Unit Test Method) player test killed. </summary>
    ///
 

    [Test]
    public void PlayerTest_Killed()
    {
        // Arrange
        var playerToDamage = new PlayerController();
        playerToDamage.MockUp();

        // Act
        playerToDamage.Damage(51.0f, null);

        // Assert
        Assert.That(playerToDamage.isDead, Is.EqualTo(true));
    }
}
