using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : LivingEntity
{
    protected Enemy enemy;

    protected Player player;

    [SerializeField]
    private int seedLoss;

    protected override void Awake()
    {
        base.Awake();

        enemy = FindObjectOfType<Enemy>();
        player = FindObjectOfType<Player>();

    }
    protected override void Die()
    {

    }

    public override void TakeDamge(float damage)
    {
        base.TakeDamge(damage);

        ScreenShake.instance.Shake();

        if (team == Team.AI)
        {
            enemy.ReduceSeed(-seedLoss);
        }
        else
        {
            player.ReduceSeed(-seedLoss);
        }
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