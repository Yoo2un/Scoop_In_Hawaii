using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultScene : MonoBehaviour
{
    public static ResultScene Instance { get; private set; }

    public TextMeshProUGUI title_text;
    public TextMeshProUGUI profit_text;
    public TextMeshProUGUI expense_text; // 지출
    public TextMeshProUGUI duty_text; // 세금
    public TextMeshProUGUI final_profit_text; // 최종 수익
    public TextMeshProUGUI money_text;
    public Button next_btn;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            if (Instance.gameObject != gameObject) Destroy(gameObject);
            else Destroy(this);
            return;
        }

        if (Instance != null)
        {
            title_text.text = $"[{DayManager.Instance.day}일차] 장사 결과 정산";
            profit_text.text = $"매출 : {EconomyManager.Instance.profit}";
            expense_text.text = $"지출 : {EconomyManager.Instance.material_cost}";
            duty_text.text = $"세금 : {EconomyManager.Instance.duty}";
            final_profit_text.text =
                $"최종 수익 : {EconomyManager.Instance.profit - EconomyManager.Instance.material_cost - EconomyManager.Instance.duty}";
            money_text.text = $"보유 자금 : {EconomyManager.Instance.GetMoney()}";

            EconomyManager.Instance.material_cost = 0;
        }
        else
        {
            title_text.text = "[?일차] 장사 결과 정산";
            profit_text.text = $"순이익 : ?";
            money_text.text = $"보유 자금 : ?";
        }

        next_btn.onClick.RemoveAllListeners();
        next_btn.onClick.AddListener(DayManager.Instance.NextDay);
    }
}
