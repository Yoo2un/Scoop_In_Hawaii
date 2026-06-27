using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultScene : MonoBehaviour
{
    public static ResultScene Instance { get; private set; }

    public TextMeshProUGUI title_text;
    public TextMeshProUGUI profit_text;
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
            title_text.text = $"[{GameManager.Instance.day}일차] 장사 결과 정산";
            profit_text.text = $"순이익 : {GameManager.Instance.profit}";
            money_text.text = $"보유 자금 : {GameManager.Instance.getMoney()}";
        }
        else
        {
            title_text.text = "[?일차] 장사 결과 정산";
            profit_text.text = $"순이익 : ?";
            money_text.text = $"보유 자금 : ?";
        }

        next_btn.onClick.RemoveAllListeners();
        next_btn.onClick.AddListener(GameManager.Instance.NextDay);
    }
}
