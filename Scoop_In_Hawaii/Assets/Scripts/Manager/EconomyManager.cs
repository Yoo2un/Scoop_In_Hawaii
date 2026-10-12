using TMPro;
using UnityEngine;
using System.Collections;

public class EconomyManager : MonoBehaviour, IDataPersistence
{
    public static EconomyManager Instance;

    [SerializeField]
    private int money = 0;
    public int profit = 0;
    public int material_cost = 0;

    public int duty = 500;
    
    [SerializeField]
    private TextMeshProUGUI moneyText;
    [SerializeField]
    private TextMeshProUGUI moneyDeltaText;

    private Coroutine deltaTextCoroutine;
    private Vector2 originalDeltaTextPosition = new Vector2(200, -1);
    [SerializeField] private float floatDistance = 50f; // ���� ������ �Ÿ�
    [SerializeField] private float floatDuration = 1.0f; // �ִϸ��̼� ���� �ð�

    public void LoadData(GameData data)
    {
        Debug.Log($"[Load] money = {data.money}");
        money = data.money;

        RefreshMoneyUI();
    }

    public void SaveData(ref GameData data)
    {
        Debug.Log($"[Save] money = {money}");
        data.money = money;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int GetMoney()
    {
        return money;
    }

    public void SetMoney(int money)
    {
        this.money = money;
        RefreshMoneyUI();
    }

    public void AddMoney(int money)
    { 
        this.money += money;    
        RefreshMoneyUI();
        ShowMoneyDelta(money);
    }

    public void SubtractMoney(int money)
    {
        this.money -= money;
        RefreshMoneyUI();
        ShowMoneyDelta(-money);
    }

    public void SetUI(TextMeshProUGUI moneyText)
    {
        this.moneyText = moneyText;

        if (this.moneyText != null)
            this.moneyText.text = money.ToString();
    }

    private void RefreshMoneyUI()
    {
        if (moneyText != null)
            moneyText.text = money.ToString();
    }

    private void ShowMoneyDelta(int deltaAmount) // �� ��ȭ�� ǥ��
    {
        if (moneyDeltaText == null) return;

        if (deltaTextCoroutine != null)
            StopCoroutine(deltaTextCoroutine);

        deltaTextCoroutine = StartCoroutine(ShowMoneyDeltaRoutine(deltaAmount));
    }

    private IEnumerator ShowMoneyDeltaRoutine(int deltaAmount)
    {
        Color startColor;

        if (deltaAmount > 0)
        {
            moneyDeltaText.text = $"+{deltaAmount:N0}";
            startColor = Color.green;
        }
        else
        {
            moneyDeltaText.text = $"{deltaAmount:N0}";
            startColor = Color.red;
        }

        RectTransform rectTransform = moneyDeltaText.GetComponent<RectTransform>();

        // �ִϸ��̼� ���� �� �׻� (200, -1) ��ġ�� �ʱ�ȭ
        rectTransform.anchoredPosition = originalDeltaTextPosition;
        moneyDeltaText.color = startColor;
        moneyDeltaText.gameObject.SetActive(true);

        Vector2 targetPos = originalDeltaTextPosition + new Vector2(0, floatDistance);

        float elapsed = 0f;

        while (elapsed < floatDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / floatDuration;

            // ��ġ �̵�: (200, -1) -> (200, -1 + floatDistance)
            rectTransform.anchoredPosition = Vector2.Lerp(originalDeltaTextPosition, targetPos, t);

            // ���İ� ���̵�ƿ� (1 -> 0)
            Color newColor = startColor;
            newColor.a = Mathf.Lerp(1f, 0f, t);
            moneyDeltaText.color = newColor;

            yield return null;
        }

        moneyDeltaText.gameObject.SetActive(false);
   
    }
}
