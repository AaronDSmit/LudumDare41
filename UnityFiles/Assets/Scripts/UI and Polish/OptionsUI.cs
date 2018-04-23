using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsUI : MonoBehaviour
{
    [SerializeField]
    private float targetY;

    private float startY;

    [SerializeField]
    private float animationTime;

    RectTransform rect;

    private bool pulledDown = false;

    [SerializeField]
    private RectTransform arrowButton;

    [SerializeField]
    private GameObject CamShakeButton;

    [SerializeField]
    private GameObject audioButton;

    private ScreenShake camShake;

    private bool audioOn;

    private bool camShakeOn;


    private void Awake()
    {
        rect = GetComponent<RectTransform>();

        camShake = FindObjectOfType<ScreenShake>();

        startY = rect.position.y;
    }


    public void ToggleAudio()
    {
        audioOn = !audioOn;

        AudioListener.volume = (audioOn) ? 1.0f : 0.0f;
    }

    public void ToggleCamShake()
    {
        camShakeOn = !camShakeOn;

        camShake.enabled = camShakeOn;
    }

    public void TogglePullDown()
    {
        if (!pulledDown)
        {
            StartCoroutine(Animate(startY, targetY));
        }
        else
        {
            StartCoroutine(Animate(targetY, startY));
        }
    }


    private IEnumerator Animate(float from, float to)
    {
        float speed = 1 / animationTime;
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime * speed;
            rect.position = new Vector3(rect.position.x, Mathf.SmoothStep(from, to, percent), rect.position.z);

            yield return null;
        }

        arrowButton.localScale = new Vector3(arrowButton.localScale.x, -arrowButton.localScale.y, arrowButton.localScale.z);

        pulledDown = !pulledDown;
    }
}