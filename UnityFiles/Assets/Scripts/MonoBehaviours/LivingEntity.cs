using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour
{
    [SerializeField]
    protected float startHealth;
    protected float currentHealth;

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

    protected void Die()
    {

    }
}