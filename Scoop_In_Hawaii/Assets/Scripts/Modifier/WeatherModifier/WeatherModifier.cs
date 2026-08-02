using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class WeatherModifier : MonoBehaviour
{
    public static WeatherModifier Instance { get; private set; }

    public WeatherType CurrentWeather { get; private set; } = WeatherType.None;

    public Volume heatWaveVolume;

    [SerializeField] private GameObject rainEffect;

    private Coroutine rainCoroutine;
    private Coroutine heatwaveCoroutine;
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

    public void StartWeather()
    {
        WeatherType weather = (Random.value < 0.5f)
            ? WeatherType.Rain
            : WeatherType.HeatWave;

        CurrentWeather = weather;

        switch (CurrentWeather)
        {
            case WeatherType.Rain:
                StartRain();
                break;

            case WeatherType.HeatWave:
                StartHeatWave();
                break;
        }
    }

    private void StartRain()
    {
        if (rainCoroutine != null)
        {
            StopCoroutine(rainCoroutine);
        }

        rainCoroutine = StartCoroutine(RainRoutine());

        Debug.Log("비가 내립니다.");
    }

    private IEnumerator RainRoutine()
    {
        CustomerManager.Instance.SetClientSpawnDelay(40f, 50f);

        rainEffect.SetActive(true);

        yield return new WaitForSeconds(60f);

        rainEffect.SetActive(false);

        CustomerManager.Instance.ResetClientSpawnDelay();

        rainCoroutine = null;
    }

    private void StartHeatWave()
    {
        if (rainCoroutine != null)
        {
            StopCoroutine(heatwaveCoroutine);
        }

        heatwaveCoroutine = StartCoroutine(HeatWaveRoutine());

        Debug.Log("폭염입니다.");
    }

    private IEnumerator HeatWaveRoutine()
    {
        CustomerManager.Instance.SetClientSpawnDelay(5f, 8f);

        heatWaveVolume.weight = 1f;

        yield return new WaitForSeconds(60f);

        CustomerManager.Instance.ResetClientSpawnDelay();

        heatWaveVolume.weight = 0f;

        heatwaveCoroutine = null;
    }
}
