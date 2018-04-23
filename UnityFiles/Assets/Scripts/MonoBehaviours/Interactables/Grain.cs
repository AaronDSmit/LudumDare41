using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grain : Interactable {
    [SerializeField] private float value;
    [SerializeField] private float existsTime;

    private Tower playerTower;
    private Tower enemyTower;

    private float currentExistsTime;

    private void Awake () {
        Tower[] towers = FindObjectsOfType<Tower>();

        for (int i = 0; i < towers.Length; i++) {
            if (towers[i].Team == Team.PLAYER)
                playerTower = towers[i];
            else
                enemyTower = towers[i];
        }
    }

    private void Update () {
        UpdateExists();
    }

    public override void Interact () {
        playerTower.HealDamage(value);
        StartCoroutine(Remove());
    }

    private void UpdateExists () {
        currentExistsTime += Time.deltaTime;

        if (currentExistsTime >= existsTime)
            GiveToEnemy();
    }

    private void GiveToEnemy () {
        enemyTower.HealDamage(value);
        StartCoroutine(Remove());
    }

    private IEnumerator Remove () {
        yield return new WaitForEndOfFrame();
        Destroy(gameObject);
    }
}
