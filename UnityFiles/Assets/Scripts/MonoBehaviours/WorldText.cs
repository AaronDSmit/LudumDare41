using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WorldText : MonoBehaviour
{
    public static WorldText instance = null;

    private Text message;

    private RectTransform image;

    [SerializeField]
    private float targetHeight;

    [SerializeField]
    private float animationTime;

    private void Awake()
    {
        // Check if instance already exists, if there isn't set instance to this otherwise destroy this.
        if (instance == null)
        {
            instance = this;

            message = GetComponentInChildren<Text>();
            image = transform.GetChild(0).GetComponent<RectTransform>();
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
}