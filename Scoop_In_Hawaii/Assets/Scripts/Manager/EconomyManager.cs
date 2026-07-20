using TMPro;
using UnityEngine;

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

    public void LoadData(GameData data)
    {
        money = data.money;

        RefreshMoneyUI();
    }

    public void SaveData(ref GameData data)
    {
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
    }

    public void SubtractMoney(int money)
    {
        this.money -= money;
        RefreshMoneyUI();
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
}
