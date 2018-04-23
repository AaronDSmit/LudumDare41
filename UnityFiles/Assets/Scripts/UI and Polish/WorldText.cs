using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WorldText : MonoBehaviour
{
    public static WorldText instance = null;

    private Text message;

    private RectTransform image;

    private Image retryButton;
    private Text retryText;

    [SerializeField]
    private float targetHeight;

    [SerializeField]
    private float fadeInTime;

    [SerializeField]
    private float animationTime;

    [SerializeField]
    private float retryButtonDisplayDelay;

    private void Awake()
    {
        // Check if instance already exists, if there isn't set instance to this otherwise destroy this.
        if (instance == null)
        {
            instance = this;

            message = GetComponentInChildren<Text>();
            image = transform.GetChild(0).GetComponent<RectTransform>();

            retryButton = transform.GetChild(1).GetComponentInChildren<Image>();
            retryText = transform.GetChild(1).GetComponentInChildren<Text>();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        // Persistant between scene loading
        DontDestroyOnLoad(gameObject);
    }

    public void ShowText(string text)
    {
        message.text = text;
        StartCoroutine(Animate());
    }

    [ContextMenu("ShowRetryButton")]
    public void ShowRetryButton()
    {
        StartCoroutine(FadeUI(Color.clear, Color.white, fadeInTime));
    }

    private void Start()
    {
        ShowText("Farm\nFight");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShowText("You\nWin");
        }
    }

    public void ResetUI()
    {
        retryButton.gameObject.SetActive(false);
    }

    private IEnumerator Animate()
    {
        image.gameObject.SetActive(true);

        float speed = 1 / animationTime;
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime * speed;
            image.sizeDelta = new Vector2(image.rect.width, Mathf.SmoothStep(0, targetHeight, percent));

            yield return null;
        }

        image.gameObject.SetActive(false);
    }


    private IEnumerator FadeUI(Color from, Color to, float time)
    {
        yield return new WaitForSeconds(retryButtonDisplayDelay);

        retryButton.gameObject.SetActive(true);

        float speed = 1 / time;
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime * speed;

            if (retryButton)
            {
                retryButton.color = new Color(retryButton.color.r, retryButton.color.g, retryButton.color.b, Mathf.Lerp(from.a, to.a, percent));
            }

            if (retryText)
            {
                retryText.color = new Color(retryText.color.r, retryText.color.g, retryText.color.b, Mathf.Lerp(from.a, to.a, percent));
            }

            yield return null;
        }

    }
}