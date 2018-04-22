using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Plant : MonoBehaviour
{
    [SerializeField]
    private int growthStage;

    [SerializeField]
    private float growthTime;

    [SerializeField]
    private GrowthVisual[] growthStages;

    [SerializeField]
    private Unit unitPrefab;

    [SerializeField]
    private Team team;

    [Header("Audio Clips")]
    private AudioClip growthSound;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public Team Team
    {
        get { return team; }

        set { team = value; }
    }

    private void Start()
    {
        Invoke("Grow", 0);
    }

    private void Grow()
    {
        if (growthStage < growthStages.Length)
        {
            transform.localPosition = growthStages[growthStage].localposition;
            SetGlobalScale(transform, growthStages[growthStage].localScale);
            growthStage++;

            if (growthSound != null)
            {
                audioSource.pitch = growthStages[growthStage].pitch;
                audioSource.PlayOneShot(growthSound);
            }

            Invoke("Grow", growthTime);
        }
        else
        {
            Unit unit = Instantiate(unitPrefab, new Vector3(transform.position.x, 0.0f, transform.position.z), Quaternion.identity);
            unit.Activate(Team);

            Destroy(gameObject);
        }
    }

    private void SetGlobalScale(Transform _transform, Vector3 globalScale)
    {
        _transform.localScale = Vector3.one;
        _transform.localScale = new Vector3(globalScale.x / _transform.lossyScale.x, globalScale.y / _transform.lossyScale.y, globalScale.z / _transform.lossyScale.z);
    }
}


[System.Serializable]
public struct GrowthVisual
{
    public Vector3 localposition;
    public Vector3 localScale;
    public float pitch;
}