using System.Collections;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    [Header("Screen Shake")]
    [SerializeField]
    private float shakeDuration;

    private float shakeTime;

    [SerializeField]
    private float shakeAmount = 0.7f;

    [SerializeField]
    private float decreaseFactor = 1.0f;

    [Header("Screen kickback")]
    [SerializeField]
    private float kickBackDuration = 0.1f;

    [SerializeField]
    private float kickBackAmount = 1;

    private Vector3 originalPos;

    private bool shaking = false;

    private bool kickedBacked = false;

    private void OnEnable()
    {
        originalPos = transform.localPosition;
        shakeTime = shakeDuration;
    }

    public void KickBack()
    {
        if (!kickedBacked)
        {
            StartCoroutine(KickBackCam(originalPos, originalPos + Vector3.back * kickBackAmount, kickBackDuration));
            kickedBacked = true;
        }
        else
        {
            Debug.Log("already being kicked back");
        }
    }

    public void Shake()
    {
        if (!shaking)
        {
            StartCoroutine(ShakeCam());
            shaking = true;
        }
    }

    private IEnumerator KickBackCam(Vector3 from, Vector3 to, float time)
    {
        float speed = 1 / time;
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime * speed;
            transform.localPosition = Vector3.Lerp(from, to, percent);

            yield return null;
        }

        speed = 1 / time;
        percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime * speed;
            transform.localPosition = Vector3.Lerp(to, from, percent);

            yield return null;
        }

        kickedBacked = false;
    }

    private IEnumerator ShakeCam()
    {
        while (shakeTime > 0)
        {
            //transform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPos + Random.insideUnitSphere * shakeAmount, Time.deltaTime * 3);
            shakeTime -= Time.deltaTime * decreaseFactor;
            yield return null;
        }

        shaking = false;
        shakeTime = shakeDuration;
        transform.localPosition = originalPos;
    }
}