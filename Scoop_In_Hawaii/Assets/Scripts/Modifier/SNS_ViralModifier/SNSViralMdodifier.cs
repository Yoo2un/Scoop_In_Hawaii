using System.Collections;
using UnityEngine;

public class SNSViralMdodifier : MonoBehaviour
{
    public static SNSViralMdodifier Instance { get; private set; }

    [SerializeField] private GameObject AlramPanel;
    [SerializeField] private float warningTime = 1.5f;

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

    private void Start()
    {
        SNS_Viral();
    }

    public void SNS_Viral()
    {
        StartCoroutine(PlaySNSModifier());
    }

    public IEnumerator PlaySNSModifier()
    {
        AlramPanel.gameObject.SetActive(true);

        yield return new WaitForSeconds(warningTime);

        AlramPanel.gameObject.SetActive(false);

        yield return new WaitForSeconds(2f);

        GameManager.Instance.SetClientSpawnDelay(2f, 4f);

        yield return new WaitForSeconds(30f);

        GameManager.Instance.ResetClientSpawnDelay();
    }
}
