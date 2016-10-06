using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

public class PlayerGoldTests:MonoBehaviour
{
    [Test]
    public void PlayerTest_PickupGold()
    {
        // Arrange
        var playerToPickup = new PlayerController();
        playerToPickup.MockUp();

        // Act
        playerToPickup.UpdateGold(2);

        // Assert
        Assert.That(playerToPickup.getGold(), Is.EqualTo(2));
    }
}
