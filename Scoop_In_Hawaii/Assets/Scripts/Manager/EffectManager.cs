using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance { get; private set; }

    [Header("Screen Effects")]
    [SerializeField] private ScreenEffect screenEffect;

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

    public void FadeOut(System.Action callback = null)
    {
        screenEffect.FadeOut(callback);
    }

    public void FadeIn()
    {
        screenEffect.FadeIn();
    }
}
