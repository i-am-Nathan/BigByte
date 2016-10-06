using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Player: BaseEntity
{
    private bool _isAttacking;

    public bool isAttacking()
    {
        return _isAttacking;
    }

    public void setAttacking(bool a)
    {
        _isAttacking = a;
    }
}
