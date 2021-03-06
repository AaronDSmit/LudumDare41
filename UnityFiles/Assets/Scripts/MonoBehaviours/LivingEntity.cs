﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team { PLAYER, AI }

[RequireComponent(typeof(AudioSource))]
public class LivingEntity : MonoBehaviour
{
    [SerializeField]
    protected float startHealth;
    protected float currentHealth;

    [SerializeField]
    protected Team team;

    [SerializeField]
    protected RectTransform hpBar;

    protected AudioSource audioSource;

    [Header("Audio Clip")]
    [SerializeField]
    protected AudioClip damaged;

    public Team Team
    {
        get { return team; }

        set { team = value; }
    }

    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    protected virtual void Start()
    {
        currentHealth = startHealth;
    }

    public virtual void HealDamage (float heal) {
        currentHealth += heal;

        currentHealth = Mathf.Clamp(currentHealth, 0, startHealth);

        UpdateHealthBar();
    }

    public virtual void TakeDamge(float damage)
    {
        currentHealth -= damage;

        currentHealth = Mathf.Clamp(currentHealth, 0, startHealth);

        if (damaged != null)
        {
            audioSource.PlayOneShot(damaged);
        }

        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    [ContextMenu("TakeDamge")]
    public void TakeDamge()
    {
        currentHealth -= 50;

        currentHealth = Mathf.Clamp(currentHealth, 0, startHealth);

        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {

    }

    protected void UpdateHealthBar () {
        if (hpBar != null) {
            hpBar.localScale = new Vector3(currentHealth / startHealth, 1, 1);
        }
    }
}