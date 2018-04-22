using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    [SerializeField] private List<Plant> plantPrefabs;
    [SerializeField] private float plotCheckTimeMin;
    [SerializeField] private float plotCheckTimeMax;

    private List<Plot> plots;
    private float currentCheckTime;
    private float checkCount;
    private bool active;

    private void Start () {
        active = true;

        plots = new List<Plot>();

        Plot[] allPlots = FindObjectsOfType<Plot>();

        for (int i = 0; i < allPlots.Length; i++) {
            if (allPlots[i].Team == Team.AI)
                plots.Add(allPlots[i]);
        }
    }

    void Update () {
        if (active == false)
            return;

        CheckPlots();
	}

    public void Activate () {
        active = true;
        RandomizeCheckTime();
    }

    public void ResetEnemy () {
        active = false;
        checkCount = 0f;
    }

    private void CheckPlots () {
        checkCount += Time.deltaTime;

        if(checkCount >= currentCheckTime) {
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
