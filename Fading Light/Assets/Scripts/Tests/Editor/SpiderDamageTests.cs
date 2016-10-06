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
