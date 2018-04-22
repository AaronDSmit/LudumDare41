using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    [SerializeField]
    private int growthStage;

    [SerializeField]
    private float growthTime;

    [SerializeField]
    private GrowthVisual[] growthStages;

    private MeshFilter meshFilter;

    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshFilter = transform.GetChild(0).GetComponent<MeshFilter>();
        meshRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        Invoke("Grow", 0);
    }

    private void Grow()
    {
        if (growthStage < growthStages.Length)
        {
            meshFilter.mesh = growthStages[growthStage].mesh;
            meshRenderer.material = growthStages[growthStage].material;
            growthStage++;

            Invoke("Grow", growthTime);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

[System.Serializable]
public struct GrowthVisual
{
    public Mesh mesh;
    public Material material;
}