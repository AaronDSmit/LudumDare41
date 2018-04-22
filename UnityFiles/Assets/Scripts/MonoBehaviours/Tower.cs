using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : LivingEntity
{
    protected override void Die()
    {
        if (team == Team.PLAYER)
        {
            Player.instance.Lose();
        }
    }
}