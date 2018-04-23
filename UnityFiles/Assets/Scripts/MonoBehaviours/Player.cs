using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class Player : MonoBehaviour
{
    public static Player instance = null;

    [SerializeField]
    private Plant[] plants;

    private int currentPlant;

    [SerializeField]
    private int startSeedCount;

    [SerializeField]
    private int plantCost;

    [SerializeField]
    private float intialStartDelay;

    private Text seedCountUI;

    [SerializeField]
    private string loseText;

    [SerializeField]
    private string winText;

    private bool inControl;

    private int seedCount;

    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip winSound;

    [SerializeField]
    private AudioClip loseSound;

    private AudioSource audioSource;

    private Plot selectedPlot;

    private bool gameStarted = false;

    private bool gameEnded = false;

    private Enemy opponent;

    private GameObject arrowTutorialUI;

    private void Awake()
    {
        inControl = false;

        // Check if instance already exists, if there isn't set instance to this otherwise destroy this.
        if (instance == null)
        {
            instance = this;

            audioSource = GetComponent<AudioSource>();

            opponent = FindObjectOfType<Enemy>();

            seedCount = startSeedCount;

            seedCountUI = GameObject.FindGameObjectWithTag("PlayerSeedCountUI").GetComponent<Text>();
            seedCountUI.text = "" + seedCount;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        // Persistent between scene loading
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Invoke("GainControl", intialStartDelay);
    }

    private void GainControl()
    {
        inControl = true;
    }

    public void Lose()
    {
        if (!gameEnded)
        {
            WorldText.instance.ShowText(loseText);
            inControl = false;

            if (loseSound != null)
            {
                audioSource.PlayOneShot(loseSound);
            }

            gameEnded = true;

            WorldText.instance.ShowRetryButton();
        }
    }

    public void Win()
    {
        if (!gameEnded)
        {
            WorldText.instance.ShowText(winText);
            inControl = false;

            if (winSound != null)
            {
                audioSource.PlayOneShot(winSound);
            }

            gameEnded = true;

            WorldText.instance.ShowRetryButton();
        }
    }

    public void ResetGame()
    {
        Tower[] towers = FindObjectsOfType<Tower>();

        for (int i = 0; i < towers.Length; i++)
        {
            towers[i].ResetTower();
        }

        Plot[] plots = FindObjectsOfType<Plot>();

        for (int i = 0; i < plots.Length; i++)
        {
            plots[i].ResetPlot();
        }

        Unit[] units = FindObjectsOfType<Unit>();

        for (int i = 0; i < units.Length; i++)
        {
            Destroy(units[i].gameObject);
        }

        opponent.ResetEnemy();

        WorldText.instance.ResetUI();

        gameStarted = false;
        selectedPlot = null;
        inControl = true;
        gameEnded = false;

        seedCount = startSeedCount;
        seedCountUI.text = "" + seedCount;

        ShowTutorialUI();
    }

    public void HideTutorialUI()
    {
        TextPingPong[] tutorialText = FindObjectsOfType<TextPingPong>();

        for (int i = 0; i < tutorialText.Length; i++)
        {
            if (tutorialText[i].transform.name == "Arrow")
            {
                arrowTutorialUI = tutorialText[i].gameObject;
            }

            tutorialText[i].FadeOut();
        }
    }

    public void ShowTutorialUI()
    {
        if (arrowTutorialUI != null)
        {
            arrowTutorialUI.SetActive(true);
        }
    }

    private void Update()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current);

        pointerData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        if (results.Count > 0)
        {
            if (results[0].gameObject.layer == LayerMask.NameToLayer("RetryUI"))
            {
                ResetGame();
            }
        }

        if (inControl && Input.GetMouseButtonDown(0))
        {
            if (results.Count > 0)
            {
                if (results[0].gameObject.layer == LayerMask.NameToLayer("CarrotUI"))
                {
                    selectedPlot.PlacePlant(plants[1], Team.PLAYER);

                    seedCount -= plantCost;
                    seedCountUI.text = "" + seedCount;

                    if (!gameStarted)
                    {
                        opponent.Activate();
                        HideTutorialUI();
                        gameStarted = true;
                    }
                }
                else if (results[0].gameObject.layer == LayerMask.NameToLayer("BeetUI"))
                {
                    selectedPlot.PlacePlant(plants[0], Team.PLAYER);
                    seedCount -= plantCost;
                    seedCountUI.text = "" + seedCount;

                    if (!gameStarted)
                    {
                        opponent.Activate();
                        HideTutorialUI();
                        gameStarted = true;
                    }
                }
                else if (results[0].gameObject.layer == LayerMask.NameToLayer("OptionsUI"))
                {
                    results[0].gameObject.GetComponentInParent<OptionsUI>().TogglePullDown();
                }
                else if (results[0].gameObject.layer == LayerMask.NameToLayer("AudioButton"))
                {
                    results[0].gameObject.GetComponentInParent<OptionsUI>().ToggleAudio();
                }
                else if (results[0].gameObject.layer == LayerMask.NameToLayer("CamShakeButton"))
                {
                    results[0].gameObject.GetComponentInParent<OptionsUI>().ToggleCamShake();
                }
                else if (results[0].gameObject.layer == LayerMask.NameToLayer("QuitButton"))
                {
                    Application.Quit();
                }
                else if (results[0].gameObject.layer == LayerMask.NameToLayer("GrainPickup"))
                {
                    Grain g = results[0].gameObject.GetComponentInParent<Grain>();
                    seedCount += g.GrainValue;
                    seedCount = Mathf.Clamp(seedCount, 0, startSeedCount);
                    seedCountUI.text = "" + seedCount;

                    g.Interact();
                }
            }
            else
            {
                if (seedCount >= plantCost)
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
}