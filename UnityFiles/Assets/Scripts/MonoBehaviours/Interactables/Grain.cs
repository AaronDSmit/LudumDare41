using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grain : Interactable {
    [SerializeField] private int grainValue;
    [SerializeField] private float existsTime;

    private Tower playerTower;
    private Tower enemyTower;

	private Enemy enemy;

    private float currentExistsTime;

	public int GrainValue {
		get {
			return grainValue;
		}
	}

	private void Awake () {
        Tower[] towers = FindObjectsOfType<Tower>();

        for (int i = 0; i < towers.Length; i++) {
            if (towers[i].Team == Team.PLAYER)
                playerTower = towers[i];
            else
                enemyTower = towers[i];
        }

		enemy = FindObjectOfType<Enemy>();
    }

    private void Update () {
        UpdateExists();
    }

    public override void Interact () {
        StartCoroutine(Remove());
    }

    private void UpdateExists () {
        currentExistsTime += Time.deltaTime;

        if (currentExistsTime >= existsTime)
            GiveToEnemy();
    }

    private void GiveToEnemy () {
		enemy.GiveGrain(grainValue);
        StartCoroutine(Remove());
    }

    private IEnumerator Remove () {
        yield return new WaitForEndOfFrame();
        Destroy(gameObject);
    }
}
