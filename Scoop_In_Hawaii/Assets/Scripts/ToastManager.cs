using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ToastManager : MonoBehaviour
{
    public static ToastManager Instance { get; private set; }

    [SerializeField] private GameObject toast_background;
    [SerializeField] private GameObject toast_message;

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Vector2 startPositionOffset = new Vector2(0, -150f);
    [SerializeField] private Vector2 endPositionOffset = new Vector2(0, 200f);

    [SerializeField] private float moveInDuration = 0.3f;
    [SerializeField] private float displayDuration = 3.0f;
    [SerializeField] private float moveOutDuration = 0.15f;

    private Vector2 _targetLocalPosition;

    private static readonly int GradientRangeID = Shader.PropertyToID("_GradientRange");

    private Coroutine timerCoroutine;

    private void Awake()
    {
        Instance = this;

        toast_message.SetActive(true);
        toast_background.SetActive(false);

        if (rectTransform == null) rectTransform = toast_background.GetComponent<RectTransform>();
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();

        _targetLocalPosition = rectTransform.anchoredPosition;
    }

    public void Toast(string message)
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }
        StopAllCoroutines();
        toast_background.SetActive(true);
        toast_message.GetComponent<TextMeshProUGUI>().text = message;

        timerCoroutine = StartCoroutine(ToastSequenceRoutine());
    }


    private IEnumerator ToastSequenceRoutine()
    {
        rectTransform.anchoredPosition = _targetLocalPosition + startPositionOffset;
        canvasGroup.alpha = 1f;

        float elapsed = 0f;
        Vector2 spawnPos = _targetLocalPosition + startPositionOffset;

        // Move In
        while (elapsed < moveInDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / moveInDuration;

            rectTransform.anchoredPosition = Vector2.Lerp(spawnPos, _targetLocalPosition, t);
            yield return null;
        }
        rectTransform.anchoredPosition = _targetLocalPosition;

        // Wait
        yield return new WaitForSeconds(displayDuration);

        elapsed = 0f;
        Vector2 exitPos = _targetLocalPosition + endPositionOffset;

        // Move Out
        while (elapsed < moveOutDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / moveOutDuration;

            rectTransform.anchoredPosition = Vector2.Lerp(_targetLocalPosition, exitPos, t);

            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t);

            yield return null;
        }

        rectTransform.anchoredPosition = exitPos;
        canvasGroup.alpha = 0f;

        toast_background.SetActive(false);
    }
}
