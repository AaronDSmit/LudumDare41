using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour
{
    private Plant plant;

    public void PlacePlant(Plant _plant)
    {
        if (plant == null)
        {
            plant = Instantiate<Plant>(_plant, transform.position + Vector3.up, Quaternion.identity);
        }
    }
}