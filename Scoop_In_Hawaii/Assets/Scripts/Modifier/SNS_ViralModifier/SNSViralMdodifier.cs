using System.Collections;
using UnityEngine;

public class SNSViralMdodifier : MonoBehaviour
{
    public static SNSViralMdodifier Instance { get; private set; }

    [SerializeField] private GameObject AlramPanel;
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

    public void SNS_Viral()
    {
        StartCoroutine(PlaySNSModifier());
    }

    public IEnumerator PlaySNSModifier()
    {
        AlramPanel.gameObject.SetActive(true);

        yield return new WaitForSeconds(AlramTiming);

        AlramPanel.gameObject.SetActive(false);

        yield return new WaitForSeconds(3f);

        GameManager.Instance.SetClientSpawnDelay(2f, 4f);

        Debug.Log("SNS Àû¿ë!");

        yield return new WaitForSeconds(10f);

        GameManager.Instance.ResetClientSpawnDelay();
    }
}
