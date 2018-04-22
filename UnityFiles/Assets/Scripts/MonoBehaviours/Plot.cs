using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour
{
    private Plant plant;

    [SerializeField]
    private Team team;

    public void PlacePlant(Plant _plant, Team placedBy)
    {
        if (plant == null && placedBy == team)
        {
            plant = Instantiate<Plant>(_plant, transform.position + Vector3.up, Quaternion.identity);
            plant.Team = team;
        }
    }
}