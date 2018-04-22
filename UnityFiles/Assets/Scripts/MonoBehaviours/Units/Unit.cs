using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Unit : LivingEntity
{
    [Header("Attacking")]
    [SerializeField] private float damage;
    [SerializeField] private float siloAttackRange;
    [SerializeField] private float unitAttackRange;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float detectionRange;
    [SerializeField] private float roationSpeed;

    private bool active;
    private bool canAttack;
    private float attackTime;

    private LivingEntity currentTarget;
    private LivingEntity finalTarget;
    private NavMeshAgent agent;
    private float currentAttackRange;

    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip activated;

    [SerializeField]
    private AudioClip attackSound;

    protected override void Awake()
    {
        base.Awake();

        canAttack = true;
        agent = GetComponent<NavMeshAgent>();
    }

    public void Activate(Team team)
    {
        Team = team;
        active = true;

        List<Tower> towers = FindObjectsOfType<Tower>().ToList();
        currentTarget = finalTarget = towers.Find(t => t.Team != Team);
        UpdateAttackRange();
        agent.SetDestination(currentTarget.transform.position);
    }

    private void Update()
    {
        if (active == false)
            return;

        CheckTarget();
        CheckDistance();
        CheckAttack();
    }

    protected override void Die()
    {
        Destroy(gameObject);
    }

    private void CheckTarget()
    {
        LivingEntity closestEnemy = GetClosestEnemyUnit();

        if (closestEnemy != null)
        {
            if (closestEnemy != currentTarget)
            {
                currentTarget = closestEnemy;
                UpdateAttackRange();
                agent.destination = currentTarget.transform.position;
            }
        }
        else
        {
            if (currentTarget != finalTarget)
            {
                currentTarget = finalTarget;
                UpdateAttackRange();
                agent.destination = currentTarget.transform.position;
            }
        }
    }

    private void CheckDistance()
    {
        if (agent.destination == transform.position)
            return;

        if (Vector3.Distance(transform.position, currentTarget.transform.position) < currentAttackRange)
        {
            agent.destination = transform.position;
            UpdateFacing();
        }
    }

    private void CheckAttack()
    {
        if (canAttack == false)
        {
            attackTime += Time.deltaTime;

            if (attackTime >= attackCooldown)
            {
                canAttack = true;
                attackTime = 0f;
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, currentTarget.transform.position) <= currentAttackRange)
                Attack();
        }
    }

    private void Attack()
    {
        if (attackSound != null)
        {
            audioSource.PlayOneShot(attackSound);
        }

        currentTarget.TakeDamge(damage);
        canAttack = false;
    }

    private Unit GetClosestEnemyUnit()
    {
        Collider[] inRangeColliders = Physics.OverlapSphere(transform.position, detectionRange);
        Unit closestUnit = null;
        float currentRange = float.MaxValue;

        for (int i = 0; i < inRangeColliders.Length; i++)
        {
            Unit u = inRangeColliders[i].GetComponent<Unit>();

            if (u == null || u == this || u.Team == Team)
                continue;

            float dist = Vector3.Distance(transform.position, u.transform.position);

            if (dist < currentRange)
            {
                closestUnit = u;
                currentRange = dist;
            }
        }

        return closestUnit;
    }

    private void UpdateAttackRange () {
        if (currentTarget is Unit)
            currentAttackRange = unitAttackRange;
        else
            currentAttackRange = siloAttackRange;
    }

    private void UpdateFacing () {
        Vector3 lookPosition = agent.destination - transform.position;
        lookPosition.y = 0f;
        Quaternion rotation = Quaternion.LookRotation(lookPosition);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, roationSpeed);
    }
}