// file:	Assets\Scripts\Tests\Editor\SpiderDamageTests.cs
//
// summary:	Implements the spider damage tests class

using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

/// <summary>   A spider damage tests. </summary>
///
/// <remarks>    . </remarks>

public class SpiderDamageTests:MonoBehaviour
{
    /// <summary>   (Unit Test Method) spider boss damaged. </summary>
    ///
 

    [Test]
    public void SpiderBoss_Damaged()
    {
        // Arrange
        var spiderToDamage = new SpiderBoss();
        spiderToDamage.MockUp();

        // Act
        spiderToDamage.Damage(2.0f, null);

        // Assert
        Assert.That(spiderToDamage.CurrentHealth, Is.EqualTo(48));
        Assert.That(spiderToDamage.isDead, Is.EqualTo(false));
    }

    /// <summary>   (Unit Test Method) spider boss killed. </summary>
    ///
 

    [Test]
    public void SpiderBoss_Killed()
    {
        // Arrange
        var playerToDamage = new SpiderBoss();
        playerToDamage.MockUp();

        // Act
        playerToDamage.Damage(51.0f, null);

        // Assert
        Assert.That(playerToDamage.isDead, Is.EqualTo(true));
    }

    /// <summary>   (Unit Test Method) spider mob damaged. </summary>
    ///
 

    [Test]
    public void SpiderMob_Damaged()
    {
        // Arrange
        var spiderToDamage = new SpiderMob();
        spiderToDamage.MockUp();

        // Act
        spiderToDamage.Damage(2.0f, null);

        // Assert
        Assert.That(spiderToDamage.CurrentHealth, Is.EqualTo(48));
        Assert.That(spiderToDamage.isDead, Is.EqualTo(false));
    }

    /// <summary>   (Unit Test Method) spider mob killed. </summary>
    ///
 

    [Test]
    public void SpiderMob_Killed()
    {
        // Arrange
        var playerToDamage = new SpiderMob();
        playerToDamage.MockUp();

        // Act
        playerToDamage.Damage(51.0f, null);

        // Assert
        Assert.That(playerToDamage.isDead, Is.EqualTo(true));
    }
}
