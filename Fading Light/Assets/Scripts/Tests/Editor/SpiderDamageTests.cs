using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

public class SpiderDamageTests:MonoBehaviour
{
    [Test]
    public void SpiderTest_Damaged()
    {
        // Arrange
        //var playerToDamage = gameObject.AddComponent<PlayerController>();
        var spiderToDamage = new SpiderBoss();
        spiderToDamage.MockUp();

        // Act
        spiderToDamage.Damage(2.0f, null);

        // Assert
        Assert.That(spiderToDamage.CurrentHealth, Is.EqualTo(48));
        Assert.That(spiderToDamage.isDead, Is.EqualTo(false));
    }

    [Test]
    public void SpiderTest_Killed()
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
