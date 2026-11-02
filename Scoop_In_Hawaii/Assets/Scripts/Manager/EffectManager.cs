using TMPro;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance { get; private set; }

    [Header("Screen Effect")]
    [SerializeField] private ScreenEffect screenEffect;

    [Header("Text Effect")]
    [SerializeField] private TextEffect textEffect;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartShakeText(TextMeshProUGUI text)
    {
        textEffect.StartShakeText(text);
    }

    public void StopShakeText(TextMeshProUGUI text)
    {
        textEffect.StopShakeText(text);
    }

    public void FadeOut(System.Action callback = null)
    {
        screenEffect.FadeOut(callback);
    }

    public void FadeIn()
    {
        screenEffect.FadeIn();
    }
}
