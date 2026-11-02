using System;
using System.Collections;
using UnityEngine;

public class ScreenEffect : MonoBehaviour
{
    [SerializeField]
    private float fadeDuration = 0.5f;

    public GameObject panel;
    private Action onCompleteCallback;

    private void Start()
    {
        if(!panel)
        {
            Debug.LogError("Panel 오브젝트를 찾을 수 없습니다.");
            throw new MissingComponentException();
        }
    }

    public void FadeOut(Action callback = null)
    {
        onCompleteCallback = callback;

        panel.SetActive(true);
        StartCoroutine(CoFadeOut());
    }

    public void FadeIn()
    {
        panel.SetActive(true);
        StartCoroutine(CoFadeIn());
    }

    IEnumerator CoFadeIn()
    {
        float elapsedTime = 0f;

        while (elapsedTime <= fadeDuration)
        {
            panel.GetComponent<CanvasRenderer>().SetAlpha(Mathf.Lerp(1f,0f, elapsedTime / fadeDuration));

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Debug.Log("FadeIn 끝");
        panel.SetActive(false);
        onCompleteCallback?.Invoke();
        yield break;

    }

    IEnumerator CoFadeOut()
    {
        float elapsedTime = 0f;

        panel.GetComponent<CanvasRenderer>().SetAlpha(0f);

        while (elapsedTime <= fadeDuration)
        {
            panel.GetComponent<CanvasRenderer>().SetAlpha(Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration));

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        panel.GetComponent<CanvasRenderer>().SetAlpha(1f);

        Debug.Log("FadeIn 끝");

        onCompleteCallback?.Invoke();
        onCompleteCallback = null;
    }

    public void RegisterCallback(Action callback)
    {
        onCompleteCallback = callback;
    }
}
