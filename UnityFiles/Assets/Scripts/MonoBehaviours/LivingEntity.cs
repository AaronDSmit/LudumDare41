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

    [SerializeField]
    protected RectTransform hpBar;

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

        if (hpBar != null)
        {
            hpBar.localScale = new Vector3(currentHealth / startHealth, 1, 1);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    [ContextMenu("TakeDamge")]
    public void TakeDamge()
    {
        currentHealth -= 50;

        if (hpBar != null)
        {
            hpBar.localScale = new Vector3(currentHealth / startHealth, 1, 1);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {

    }
}