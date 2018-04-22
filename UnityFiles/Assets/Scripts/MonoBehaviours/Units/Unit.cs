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

    protected override void Start () {
        base.Start();

        canAttack = true;
        agent = GetComponent<NavMeshAgent>();

        Activate(Team.PLAYER);
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
        // can we see if there is an enemy within our detection range
        LivingEntity closestEnemy = GetClosestEnemyUnit();

        Debug.Log(closestEnemy);

        if(closestEnemy != null) {
            // if so our destination becomes their location
            if (closestEnemy != currentTarget) {
                currentTarget = closestEnemy;
                agent.destination = currentTarget.transform.position;
            }
        } else {
            // if not move to our final destination making sure not to overshoot
            if (currentTarget != finalTarget) {
                currentTarget = finalTarget;
                agent.destination = currentTarget.transform.position;
            }
        }
    }

    private void CheckDistance () {
        if (agent.destination == transform.position)
            return;

        if(Vector3.Distance(transform.position, agent.destination) <= attackRange) {
            agent.destination = transform.position;
        }
    }

    private void CheckAttack () {
        if (currentTarget == null)
            return;

        if(canAttack == false) {
            attackTime += Time.deltaTime;

            if(attackTime >= attackCooldown) {
                canAttack = true;
                attackTime = 0f;
            }
        } else {
            if(Vector3.Distance(transform.position, currentTarget.transform.position) <= attackRange) {
                currentTarget.TakeDamge(damage);
                canAttack = false;
            }
        }
    }

    private LivingEntity GetClosestEnemyUnit () {
        Collider[] inRangeColliders = Physics.OverlapSphere(transform.position, detectionRange);
        List<Unit> inRangeUnits = new List<Unit>();
        LivingEntity closestUnit = null;
        float currentRange = float.MaxValue;

        for (int i = 0; i < inRangeColliders.Length; i++) {
            LivingEntity u = inRangeColliders[i].GetComponent<LivingEntity>();

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