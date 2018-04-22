using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour
{
    private Plant plant;

    [SerializeField]
    private Team team;

    private GameObject placementPopup;

    private void Awake()
    {
        if (team == Team.PLAYER)
        {
            placementPopup = transform.GetChild(1).gameObject;
        }
    }

    public void ShowPopup()
    {
        if (plant == null && placementPopup != null)
            placementPopup.SetActive(true);
    }

    public void HidePopup()
    {
        if (placementPopup != null)
            placementPopup.SetActive(false);
    }

    public void ResetPlot()
    {
        if (plant != null)
        {
            Destroy(plant.gameObject);
            plant = null;
        }

        if (placementPopup != null)
        {
            placementPopup.SetActive(false);
        }
    }

    public Plant Plant
    {
        get { return plant; }
    }

    public Team Team
    {
        get
        {
            return team;
        }
    }

    public void PlacePlant(Plant _plant, Team placedBy)
    {
        if (plant == null && placedBy == team)
        {
            if (team == Team.PLAYER)
            {
                plant = Instantiate(_plant, transform.position + Vector3.up, Quaternion.Euler(0.0f, 90.0f, 0.0f), transform);
            }
            else
            {
                plant = Instantiate(_plant, transform.position + Vector3.up, Quaternion.Euler(0.0f, 270.0f, 0.0f), transform);
            }

            plant.Team = team;

            if (placementPopup != null)
            {
                placementPopup.SetActive(false);
            }
        }
    }
}