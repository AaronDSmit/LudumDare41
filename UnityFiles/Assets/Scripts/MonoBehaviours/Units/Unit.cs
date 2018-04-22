using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Unit : LivingEntity {
    [Header("Attacking")]
    [SerializeField] private int damage;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float detectionRange;

    private bool canAttack;
    private float attackTime;

    private LivingEntity currentTarget;
    private LivingEntity finalTarget;
    private NavMeshAgent agent;

    protected override void Start () {
        base.Start();

        canAttack = true;
        agent = GetComponent<NavMeshAgent>();
    }

    public void Activate (Team team) {
        Team = team;
        List<Tower> towers = FindObjectsOfType<Tower>().ToList();

        currentTarget = finalTarget = towers.Find(t => t.Team == Team);
        agent.SetDestination(currentTarget.transform.position);
    }

    private void Update () {
        CheckDestination();
        CheckAttack();
    }

    private void CheckDestination () {
        // can we see if there is an enemy within our detection range
        // if so our destination becomes their location
        // if not move to our final destination
    }

    private void CheckAttack () {
        if(canAttack == false) {
            attackTime += Time.deltaTime;

            //if(attackTime >= )

            return;
        } 
    }
}