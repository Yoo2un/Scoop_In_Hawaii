using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TrendFlavorModifier : MonoBehaviour
{
    public static TrendFlavorModifier Instance { get; private set; }

    public Flavor CurrentTrendFlavor { get; private set; }
    public IceCreamType CurrentTrendType { get; private set; }

    [SerializeField] private float duration = 60f;
    [SerializeField] private TextMeshProUGUI trendText;
    [SerializeField] private GameObject trendPanel;
    [SerializeField] private float AlramTiming = 1.5f;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void StartTrendFlavor()
    {
        Debug.Log("¸À ¿­Ç³ ½ÃÀÛ!");
        StartCoroutine(TrendRoutine());
    }

    private IEnumerator TrendRoutine()
    {
        Debug.Log("TrendRoutine ½ÇÇà");
        Debug.Log($"CurrentTrendFlavor ¼³Á¤ : {CurrentTrendFlavor}");

        CurrentTrendType = (IceCreamType)Random.Range(0, 2);

        List<Flavor> flavors = IceCreamData.TypeFlavors[CurrentTrendType];

        CurrentTrendFlavor = flavors[Random.Range(0, flavors.Count)];

        trendText.text = $"{IceCreamData.FlavorNames[CurrentTrendFlavor]} ¸À ¿­Ç³!";

        trendPanel.SetActive(true);

        yield return new WaitForSeconds(AlramTiming);

        trendPanel.SetActive(false);

        yield return new WaitForSeconds(duration);

        Debug.Log("¸ðµðÆÄÀÌ¾î Á¾·á");

        CurrentTrendFlavor = Flavor.None;

    }
}
