using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Plant[] plants;

    [SerializeField]
    private int currentPlant;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Plot plot = hit.transform.GetComponent<Plot>();

                if (plot != null)
                {
                    plot.PlacePlant(plants[currentPlant]);
                }
            }
        }
    }
}