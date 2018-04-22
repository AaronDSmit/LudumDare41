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

    [SerializeField]
    private GameObject loseUI;

    [SerializeField]
    private GameObject winUI;

    private bool inControl;

    private void Awake()
    {
        inControl = true;

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
        loseUI.SetActive(true);
        inControl = false;
    }

    public void Win()
    {
        winUI.SetActive(true);
        inControl = false;
    }

    private void Update()
    {
        if (inControl && Input.GetMouseButton(0))
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