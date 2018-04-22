using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team { PLAYER, AI }

public class LivingEntity : MonoBehaviour
{
    [SerializeField]
    protected float startHealth;
    protected float currentHealth;

    [SerializeField]
    protected Team team;

    public Team Team
    {
        get { return team; }

        set { team = value; }
    }

    protected virtual void Start()
    {
        currentHealth = startHealth;
    }

    public void TakeDamge(float damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0)
        {
            Die();
        }
    }

    [ContextMenu("TakeDamge")]
    public void TakeDamge()
    {
        currentHealth -= 1;

        transform.Translate(Vector3.down);

        if (currentHealth < 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {

    }
}