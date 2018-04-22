using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    [SerializeField] private List<Plant> plantPrefabs;
    [SerializeField] private List<Plot> plots;
    [SerializeField] private float plotCheckTime;

    private float checkCount;
    private bool active;

    private void Start () {
        active = true;
    }

    void Update () {
        if (active == false)
            return;

        CheckPlots();
	}

    public void Activate () {
        active = true;
    }

    private void CheckPlots () {
        checkCount += Time.deltaTime;

        if(checkCount >= plotCheckTime) {
            checkCount = 0f;
            PlacePlant();
        }
    }

    private void PlacePlant () {
        // Check all plots at random
        // if we can find an empty one plant a plant
        for (int i = 0; i < 10000; i++) {
            Plot p = plots[Random.Range(0, plots.Count)];

            if(p.Plant == null) {
                p.PlacePlant(plantPrefabs[Random.Range(0, plantPrefabs.Count)], Team.AI);
                break;
            }
        }
    }
}
