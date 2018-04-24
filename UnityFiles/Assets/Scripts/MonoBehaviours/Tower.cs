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
        else
        {
            Player.instance.Win();
        }
    }

    public override void TakeDamge(float damage)
    {
        base.TakeDamge(damage);

        ScreenShake.instance.Shake();
    }

    public void ResetTower()
    {
        currentHealth = startHealth;

        if (hpBar != null)
        {
            hpBar.localScale = new Vector3(currentHealth / startHealth, 1, 1);
        }
    }
}