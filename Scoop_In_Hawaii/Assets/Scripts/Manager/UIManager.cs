using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    GameObject obj_Day;
    GameObject obj_Num;

    TextMeshProUGUI text_Day;
    TextMeshProUGUI text_Num;
    TextMeshProUGUI timeText;

    Vector3 day_pre_Position;
    Vector3 num_pre_Position;

    RectTransform day_RectTransform;
    RectTransform num_RectTransform;

    private Coroutine shrinkCoroutine;
    private Coroutine posCoroutine;
    private TextMeshProUGUI moneyText;
    private GameObject chat;
    private TextMeshProUGUI chatText;
    private Button dayButton;

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

    /// <summary>
    /// DayScene의 UI 오브젝트를 초기화하고
    /// 각 Manager와 UI를 연결한다.
    /// </summary>
    public void Initialize()
    {
        obj_Day = GameObject.Find("Day_Text_Day");
        obj_Num = GameObject.Find("Day_Text_Num");
        GameObject objHour = GameObject.Find("Time_Text_Hour");

        obj_Day.SetActive(false);
        obj_Num.SetActive(false);

        text_Day = obj_Day.GetComponent<TextMeshProUGUI>();
        text_Num = obj_Num.GetComponent<TextMeshProUGUI>();
        timeText = objHour.GetComponent<TextMeshProUGUI>();
        text_Num.text = DayManager.Instance.day.ToString();

        day_RectTransform = obj_Day.GetComponent<RectTransform>();
        num_RectTransform = obj_Num.GetComponent<RectTransform>();

        moneyText = GameObject.Find("Money_Text").GetComponent<TextMeshProUGUI>();

        chat = GameObject.Find("UI_Chat");
        chatText = GameObject.Find("Text_Chat").GetComponent<TextMeshProUGUI>();

        dayButton = GameObject.Find("DayCtrlBtn").GetComponent<Button>();

        EconomyManager.Instance.SetUI(moneyText);

        chat.SetActive(false);
        CustomerManager.Instance.SetUI(chat, chatText);
    }

    /// <summary>
    /// 하루 진행 버튼의 이벤트와
    /// 버튼 텍스트 및 색상을 변경한다.
    /// </summary>
    public void SetDayButton(UnityAction action, string text, Color32 color)
    {
        dayButton.onClick.RemoveAllListeners();
        dayButton.onClick.AddListener(action);

        dayButton.GetComponent<Image>().color = color;
        dayButton.GetComponentInChildren<TextMeshProUGUI>().text = text;
    }

    /// <summary>
    /// 하루 시작 시 Day UI의
    /// 등장 애니메이션을 실행한다.
    /// </summary>

    public void PlayIntro()
    {
        Invoke(nameof(Text_Day_Function), 2f);
        Invoke(nameof(Text_Num_Function), 3f);

        StartCoroutine(StartShrink(5f));
        StartCoroutine(StartTextPosReset(5f));
    }

    /// <summary>
    /// 'Day' 텍스트를 화면에 표시하고
    /// 애니메이션 시작 위치를 설정한다.
    /// </summary>
    void Text_Day_Function()
    {
        day_pre_Position = day_RectTransform.position;

        text_Day.fontSize = 120;
        day_RectTransform.position = GameObject.Find("Pos_Day").GetComponent<RectTransform>().position;
        obj_Day.SetActive(true);
    }

    /// <summary>
    /// 날짜 숫자 텍스트를 화면에 표시하고
    /// 애니메이션 시작 위치를 설정한다.
    /// </summary>
    void Text_Num_Function()
    {
        num_pre_Position = num_RectTransform.position;

        text_Num.fontSize = 120;
        num_RectTransform.position = GameObject.Find("Pos_Num").GetComponent<RectTransform>().position;
        obj_Num.SetActive(true);
    }

    /// <summary>
    /// 현재 게임 시간을
    /// 시간 UI에 갱신한다.
    /// </summary>
    public void RefreshTime()
    {
        timeText.text = $"{DayManager.Instance.GetHour()}:{DayManager.Instance.GetMinute():D2} PM";
    }

    public void RefreshDay()
    {
        text_Num.text = DayManager.Instance.day.ToString();
    }

    /// <summary>
    /// 지정한 시간만큼 대기한 후
    /// 텍스트 축소 애니메이션을 시작한다.
    /// </summary>
    public IEnumerator StartShrink(float delay)
    {
        yield return new WaitForSeconds(delay);
        Shrink(36f, 5.0f);
    }

    /// <summary>
    /// Day 텍스트의
    /// 크기 축소 애니메이션을 실행한다.
    /// </summary>
    public void Shrink(float targetSize, float duration)
    {
        if (shrinkCoroutine != null) StopCoroutine(shrinkCoroutine);

        shrinkCoroutine = StartCoroutine(ShrinkProcess(targetSize, duration));
    }

    /// <summary>
    /// Day 텍스트의 크기를
    /// 목표 크기까지 부드럽게 변경한다.
    /// </summary>
    private IEnumerator ShrinkProcess(float targetSize, float duration)
    {
        float day_StartSize = text_Day.fontSize;
        float num_StartSize = text_Num.fontSize;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            text_Day.fontSize = Mathf.Lerp(day_StartSize, targetSize, elapsed / duration);
            text_Num.fontSize = Mathf.Lerp(num_StartSize, targetSize, elapsed / duration);
            yield return null;
        }

        text_Day.fontSize = targetSize;
        text_Day.fontSize = targetSize;
    }

    /// <summary>
    /// 지정한 시간만큼 대기한 후
    /// 텍스트 위치 복귀 애니메이션을 시작한다.
    /// </summary>
    public IEnumerator StartTextPosReset(float delay)
    {
        yield return new WaitForSeconds(delay);
        TextPosReset(3.0f);
    }

    /// <summary>
    /// Day 텍스트를
    /// 원래 위치로 이동시킨다.
    /// </summary>
    public void TextPosReset(float duration)
    {
        if (posCoroutine != null) StopCoroutine(posCoroutine);

        posCoroutine = StartCoroutine(TextPosResetProcess(duration));
    }

    /// <summary>
    /// Day 텍스트를
    /// 원래 위치로 부드럽게 이동시킨다.
    /// </summary>
    private IEnumerator TextPosResetProcess(float duration)
    {
        Vector3 day_Start_Pos = day_RectTransform.position;
        Vector3 num_Start_Pos = num_RectTransform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            day_RectTransform.position = Vector3.Lerp(day_Start_Pos, day_pre_Position, elapsed / duration);
            num_RectTransform.position = Vector3.Lerp(num_Start_Pos, num_pre_Position, elapsed / duration);
            yield return null;
        }

        day_RectTransform.position = day_pre_Position;
        num_RectTransform.position = num_pre_Position;
    }
}
