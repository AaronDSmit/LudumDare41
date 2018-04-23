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

    public static ScreenShake instance = null;

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

        // Persistent between scene loading
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        originalPos = transform.localPosition;
        shakeTime = shakeDuration;
    }

    public void Shake()
    {
        if (!shaking)
        {
            StartCoroutine(ShakeCam());
            shaking = true;
        }
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