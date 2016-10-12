using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Superclass for players
/// </summary>
public class Player: BaseEntity
{
    private bool _isAttacking;

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
}
