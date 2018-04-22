using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

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

    private Plot selectedPlot;

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

    [ContextMenu("ResetGame")]
    public void ResetGame()
    {
        // GameObject.FindGameObjectWithTag();

        Tower[] towers = FindObjectsOfType<Tower>();

        for (int i = 0; i < towers.Length; i++)
        {
            towers[i].ResetTower();
        }

        Unit[] units = FindObjectsOfType<Unit>();

        for (int i = 0; i < units.Length; i++)
        {
            Destroy(units[i].gameObject);
        }

        Plot[] plots = FindObjectsOfType<Plot>();

        for (int i = 0; i < plots.Length; i++)
        {
            plots[i].ResetPlot();
        }
    }

    private void Update()
    {
        if (inControl && Input.GetMouseButtonDown(0))
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);

            pointerData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            if (results.Count > 0)
            {
                if (results[0].gameObject.layer == LayerMask.NameToLayer("CarrotUI"))
                {
                    selectedPlot.PlacePlant(plants[1], Team.PLAYER);
                }
                else if (results[0].gameObject.layer == LayerMask.NameToLayer("BeetUI"))
                {
                    selectedPlot.PlacePlant(plants[0], Team.PLAYER);
                }
            }
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    Plot plot = hit.transform.GetComponent<Plot>();

                    if (plot != null)
                    {
                        if (selectedPlot == null)
                        {
                            selectedPlot = plot;
                            selectedPlot.ShowPopup();
                        }
                        else
                        {
                            selectedPlot.HidePopup();

                            if (selectedPlot != plot)
                            {
                                selectedPlot = plot;
                                selectedPlot.ShowPopup();
                            }
                            else
                            {
                                selectedPlot = null;
                            }
                        }
                    }
                }
            }
        }
    }
}