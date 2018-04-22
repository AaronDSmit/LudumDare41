using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    [SerializeField] private List<Plant> plantPrefabs;
    [SerializeField] private List<Plot> plots;
    [SerializeField] private float plotCheckTimeMin;
    [SerializeField] private float plotCheckTimeMax;

    private float currentCheckTime;
    private float checkCount;
    private bool active;

    private void Start () {
        active = true;
        RandomizeCheckTime();
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

        if(checkCount >= plotCheckTimeMax) {
            checkCount = 0f;
            RandomizeCheckTime();
            PlacePlant();
        }
    }

    private void PlacePlant () {
        for (int i = 0; i < 100; i++) {
            Plot p = plots[Random.Range(0, plots.Count)];

            if(p.Plant == null) {
                p.PlacePlant(plantPrefabs[Random.Range(0, plantPrefabs.Count)], Team.AI);
                break;
            }
        }
    }

    private void RandomizeCheckTime () {
        currentCheckTime = Random.Range(plotCheckTimeMin, plotCheckTimeMax);
    }
}
