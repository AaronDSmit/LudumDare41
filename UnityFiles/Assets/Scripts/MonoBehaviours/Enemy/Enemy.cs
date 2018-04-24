using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private List<Plant> plantPrefabs;
    [SerializeField] private float plotCheckTimeMin;
    [SerializeField] private float plotCheckTimeMax;

    [Header("Grain")]
    [SerializeField]
    private int startingGrainCount;
    [Tooltip("While at or below this amount the enemy will not spawn Units"), SerializeField] private int minGrainCount;
    [SerializeField] private int plantCost;

    private List<Plot> plots;
    private float currentCheckTime;
    private float checkCount;
    private bool active;

    private Text enemyGrainUI;
    private int currentGrain;

    [SerializeField]
    private int seedRegen;

    [SerializeField]
    private float seedRegenRate;

    private void Awake()
    {
        enemyGrainUI = GameObject.FindWithTag("EnemySeedCountUI").GetComponent<Text>();
    }

    private void Start()
    {
        plots = new List<Plot>();

        Plot[] allPlots = FindObjectsOfType<Plot>();

        for (int i = 0; i < allPlots.Length; i++)
        {
            if (allPlots[i].Team == Team.AI)
                plots.Add(allPlots[i]);
        }

        currentGrain = startingGrainCount;
        UpdateGrainUI();

        InvokeRepeating("RegenSeeds", seedRegenRate, seedRegenRate);
    }


    public void ReduceSeed(int amount)
    {
        ChangeSeedCount(-amount);
    }

    public float HPRatio()
    {
        return (float)currentGrain / startingGrainCount;
    }

    private void ChangeSeedCount(int amount)
    {
        currentGrain += amount;
        currentGrain = Mathf.Clamp(currentGrain, 0, startingGrainCount);
        UpdateGrainUI();

        if (currentGrain <= 0)
        {
            Player.instance.Win();
        }
    }


    private void RegenSeeds()
    {
        ChangeSeedCount(seedRegen);
    }

    private void Update()
    {
        if (active == false)
            return;

        CheckPlots();
    }

    public void Activate()
    {
        active = true;
        RandomizeCheckTime();
    }

    public void ResetEnemy()
    {
        active = false;
        checkCount = 0f;
        currentGrain = startingGrainCount;
        UpdateGrainUI();
    }

    public void GiveGrain(int amount)
    {
        ChangeSeedCount(amount);
    }

    private void CheckPlots()
    {
        checkCount += Time.deltaTime;

        if (checkCount >= currentCheckTime)
        {
            checkCount = 0f;
            RandomizeCheckTime();
            TryPlacePlant();
        }
    }

    private void TryPlacePlant()
    {
        if (currentGrain <= minGrainCount || currentGrain < plantCost)
            return;

        currentGrain -= plantCost;
        UpdateGrainUI();

        for (int i = 0; i < 100; i++)
        {
            Plot p = plots[Random.Range(0, plots.Count)];

            if (p.Plant == null)
            {
                p.PlacePlant(plantPrefabs[Random.Range(0, plantPrefabs.Count)], Team.AI);
                break;
            }
        }
    }

    private void RandomizeCheckTime()
    {
        currentCheckTime = Random.Range(plotCheckTimeMin, plotCheckTimeMax);
    }

    private void UpdateGrainUI()
    {
        enemyGrainUI.text = "" + currentGrain;
    }
}
