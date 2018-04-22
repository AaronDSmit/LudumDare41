using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Player : MonoBehaviour
{
    public static Player instance = null;

    [SerializeField]
    private Plant[] plants;

    [SerializeField]
    private int currentPlant;

    [SerializeField]
    private string loseText;

    [SerializeField]
    private string winText;

    private bool inControl;

    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip winSound;

    [SerializeField]
    private AudioClip loseSound;

    private AudioSource audioSource;

    private void Awake()
    {
        inControl = true;

        // Check if instance already exists, if there isn't set instance to this otherwise destroy this.
        if (instance == null)
        {
            instance = this;

            audioSource = GetComponent<AudioSource>();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        // Persistent between scene loading
        DontDestroyOnLoad(gameObject);
    }

    public void Lose()
    {
        WorldText.instance.ShowText(loseText);
        inControl = false;

        if (loseSound != null)
        {
            audioSource.PlayOneShot(loseSound);
        }
    }

    public void Win()
    {
        WorldText.instance.ShowText(winText);
        inControl = false;

        if (winSound != null)
        {
            audioSource.PlayOneShot(winSound);
        }
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
                    plot.PlacePlant(plants[Random.Range(0, 2)], Team.PLAYER);
                }
            }
        }
    }
}