using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextPingPong : MonoBehaviour
{
    RectTransform rect;

    [SerializeField]
    private float targetX;

    [SerializeField]
    private float targetY;

    [SerializeField]
    private float animationTime;

    [SerializeField]
    private float fadeTime;

    [SerializeField]
    private float initialDisplayDelay;

    private float targetValue;

    private float startValue;

    private bool ping;

    private Text text;
    private Image image;
    private Shadow[] shadows;

    private bool firstFadeIn = true;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        text = GetComponentInChildren<Text>();
        image = GetComponentInChildren<Image>();
        shadows = GetComponentsInChildren<Shadow>();
    }

    private void Start()
    {
        if (image)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0.0f);
        }

        if (text)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, 0.0f);
        }

        for (int i = 0; i < shadows.Length; i++)
        {
            shadows[i].effectColor = new Color(shadows[i].effectColor.r, shadows[i].effectColor.g, shadows[i].effectColor.b, 0.0f);
        }

        Invoke("FadeIn", initialDisplayDelay);
        firstFadeIn = false;
    }

    private void OnEnable()
    {
        if (!firstFadeIn)
        {
            FadeIn();
        }

        ping = true;

        if (targetX != 0)
        {
            startValue = rect.position.x;
            targetValue = targetX;
        }
        else if (targetY != 0)
        {
            startValue = rect.position.y;
            targetValue = targetY;
        }

        StartCoroutine(Bounce(startValue, targetValue));
    }

    public void FadeInUI()
    {
        FadeIn();
        StartCoroutine(Bounce(startValue, targetValue));
    }

    public void FadeIn()
    {
        StartCoroutine(FadeUI(Color.clear, Color.white, fadeTime));
    }

    public void FadeOut()
    {
        StartCoroutine(FadeUI(Color.white, Color.clear, fadeTime));
    }

    #region Coroutines
    private IEnumerator FadeUI(Color from, Color to, float time)
    {
        float speed = 1 / time;
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime * speed;

            if (image)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.Lerp(from.a, to.a, percent));
            }

            if (text)
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.Lerp(from.a, to.a, percent));
            }

            for (int i = 0; i < shadows.Length; i++)
            {
                shadows[i].effectColor = new Color(shadows[i].effectColor.r, shadows[i].effectColor.g, shadows[i].effectColor.b, Mathf.Lerp(from.a, to.a, percent));
            }

            yield return null;
        }

    }

    private IEnumerator Bounce(float from, float to)
    {
        float speed = 1 / animationTime;
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime * speed;

            if (targetX != 0)
            {
                rect.position = new Vector3(Mathf.SmoothStep(from, to, percent), rect.position.y, rect.position.z);
            }
            else if (targetY != 0)
            {
                rect.position = new Vector3(rect.position.x, Mathf.SmoothStep(from, to, percent), rect.position.z);
            }

            yield return null;
        }

        if (ping)
        {
            StartCoroutine(Bounce(targetValue, startValue));
            ping = false;
        }
        else
        {
            StartCoroutine(Bounce(startValue, targetValue));
            ping = true;
        }
    }

    #endregion
}