using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Unit : LivingEntity {
    [Header("Attacking")]
    [SerializeField] private float damage;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float detectionRange;

    private bool active;
    private bool canAttack;
    private float attackTime;

    private LivingEntity currentTarget;
    private LivingEntity finalTarget;
    private NavMeshAgent agent;

    protected void Awake () {
        canAttack = true;
        agent = GetComponent<NavMeshAgent>();
    }

    protected override void Start () {
        base.Start();
    }

    public void Activate (Team team) {
        Team = team;
        active = true;

        List<Tower> towers = FindObjectsOfType<Tower>().ToList();
        currentTarget = finalTarget = towers.Find(t => t.Team != Team);
        agent.SetDestination(currentTarget.transform.position);
    }

    private void Update () {
        if (active == false)
            return;

        CheckTarget();
        CheckDistance();
        CheckAttack();
    }

    protected override void Die () {
        Destroy(gameObject);
    }

    private void CheckTarget () {
        LivingEntity closestEnemy = GetClosestEnemyUnit();

        if(closestEnemy != null) {
            if (closestEnemy != currentTarget) {
                currentTarget = closestEnemy;
                agent.destination = currentTarget.transform.position;
            }
        } else {
            if (currentTarget != finalTarget) {
                currentTarget = finalTarget;
                agent.destination = currentTarget.transform.position;
            }
        }
    }

    private void CheckDistance () {
        if (agent.destination == transform.position)
            return;

        if(Vector3.Distance(transform.position, currentTarget.transform.position) < attackRange) {
            agent.destination = transform.position;
        }
    }

    private void CheckAttack () {

        if(canAttack == false) {
            attackTime += Time.deltaTime;

            if(attackTime >= attackCooldown) {
                canAttack = true;
                attackTime = 0f;
            }
        } else {
            if(Vector3.Distance(transform.position, currentTarget.transform.position) <= attackRange)
                Attack();
        }
    }

    private void Attack () {
        currentTarget.TakeDamge(damage);
        canAttack = false;
    }

    private Unit GetClosestEnemyUnit () {
        Collider[] inRangeColliders = Physics.OverlapSphere(transform.position, detectionRange);
        Unit closestUnit = null;
        float currentRange = float.MaxValue;

        for (int i = 0; i < inRangeColliders.Length; i++) {
            Unit u = inRangeColliders[i].GetComponent<Unit>();

            if (u == null || u == this || u.Team == Team)
                continue;

            float dist = Vector3.Distance(transform.position, u.transform.position);

            if (dist < currentRange) {
                closestUnit = u;
                currentRange = dist;
            }
        }

        return closestUnit;
    }
}