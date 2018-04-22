using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextPingPong : MonoBehaviour
{
    RectTransform rect;

    [SerializeField]
    private float targetX;

    [SerializeField]
    private float targetY;

    [SerializeField]
    private float animationTime;

    private float targetValue;

    private float startValue;

    private bool ping;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void Start()
    {
        ping = true;

        if (targetX != 0)
        {
            startValue = rect.position.x;
            targetValue = targetX;
        }
        else if(targetY != 0)
        {
            startValue = rect.position.y;
            targetValue = targetY;
        }

        StartCoroutine(Bounce(startValue, targetValue));
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
}