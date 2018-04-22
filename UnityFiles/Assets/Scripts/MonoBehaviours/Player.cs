using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance = null;

    [SerializeField]
    private Plant[] plants;

    [SerializeField]
    private int currentPlant;

    private void Awake()
    {
        // Check if instance already exists, if there isn't set instance to this otherwise destroy this.
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        // Persistant between scene loading
        DontDestroyOnLoad(gameObject);
    }

    public void Lose()
    {

    }

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