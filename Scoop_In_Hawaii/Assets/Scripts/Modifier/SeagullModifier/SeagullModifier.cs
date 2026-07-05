using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SeagullModifier : MonoBehaviour
{
    public static SeagullModifier Instance { get; private set; }

    [SerializeField] private Image SeagullImage;
    [SerializeField] private RectTransform rectransform;

    [SerializeField] private Vector2 startPos;
    [SerializeField] private Vector2 endPos;

    [SerializeField] private float moveTime = 2f;

    [SerializeField] private GameObject WarningPanel;
    [SerializeField] private float AlramTiming = 1.5f;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void FlySeagull()
    {
        StartCoroutine(FlyRoutine());
    }

    private IEnumerator FlyRoutine()
    {
        WarningPanel.gameObject.SetActive(true);

        yield return new WaitForSeconds(AlramTiming);

        WarningPanel.gameObject.SetActive(false);

        yield return new WaitForSeconds(2f);

        SeagullImage.gameObject.SetActive(true);

        rectransform.anchoredPosition = startPos;

        float time = 0;

        while (time < moveTime)
        {
            time += Time.deltaTime;

            rectransform.anchoredPosition = Vector2.Lerp(startPos, endPos, time / moveTime);

            yield return null;
        }

        rectransform.anchoredPosition = endPos;

        SeagullImage.gameObject.SetActive(false);
    }
}
