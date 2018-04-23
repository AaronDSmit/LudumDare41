using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grain : Interactable {
    [SerializeField] private int value;

    private Tower playerTower;
    private Tower enemyTower;

    private void Awake () {
        Tower[] towers = FindObjectsOfType<Tower>();

        for (int i = 0; i < towers.Length; i++) {
            if (towers[i].Team == Team.PLAYER)
                playerTower = towers[i];
            else
                enemyTower = towers[i];
        }
    }

    public override void Interact (Team team) {
        if(team == Team.PLAYER) {
           // playerTower.HealDamge(value);
        } else {
            // enemyTower.HealDamge(value);
        }
    }
}
