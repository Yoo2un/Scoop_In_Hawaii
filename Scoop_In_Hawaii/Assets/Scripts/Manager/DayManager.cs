using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class DayManager : MonoBehaviour
{
    // 싱글톤
    public static DayManager Instance;

    // 현재 하루 진행 상태 (아침, 영업 중, 영업 종료 등)
    public DayState dayState;

    // 현재 날짜
    public int day = 1;

    // 현재 게임 시간 [시, 분]
    int[] time = new int[2] { 11, 55 };

    // 시간 흐름 코루틴
    Coroutine timeCoroutine;

    // 시간 UI
    [SerializeField] private TextMeshProUGUI timeText;

    // 날짜 UI
    [SerializeField] private TextMeshProUGUI dayText;


    void Awake()
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
    /// 게임 시간 흐름을 시작한다.
    /// </summary>
    public void StartTime()
    {
        if (timeCoroutine != null) StopCoroutine(timeCoroutine);

        timeCoroutine = StartCoroutine(TimePasses());
    }

    /// <summary>
    /// 게임 시간 흐름을 중지한다.
    /// </summary>
    public void StopTime()
    {
        if (timeCoroutine != null)
        {
            StopCoroutine(timeCoroutine);
            timeCoroutine = null;
        }
    }

    /// <summary>
    /// 하루를 넘기고 DayScene을 다시 로드한다.
    /// </summary>
    public void NextDay()
    {
        day++;
        SceneManager.LoadScene("DayScene");
    }

    /// <summary>
    /// 현재 하루 진행 상태를 변경한다.
    /// </summary>
    public void SetState(DayState state)
    {
        dayState = state;
    }

    /// <summary>
    /// 게임 시간을 1초마다 증가시키고
    /// 시간 및 날짜 UI를 갱신한다.
    /// </summary>
    IEnumerator TimePasses()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            time[1]++;

            if (time[1] >= 60)
            {
                time[1] = 0;
                time[0]++;
            }

            if (timeText != null)
                timeText.text = $"{time[0]}:{time[1]:D2} PM";

            if (dayText != null)
                dayText.text = $"{day}";
        }
    }

    /// <summary>
    /// 하루가 시작될 때 시간을 초기화하고
    /// 상태를 Morning으로 변경한다.
    /// </summary>
    public void ResetDay()
    {
        time[0] = 11;
        time[1] = 55;

        dayState = DayState.Morning;

        if (timeText != null)
            timeText.text = "11:55 PM";

        if (dayText != null)
            dayText.text = day.ToString();
    }

    /// <summary>
    /// Day UI를 연결한다.
    /// </summary>
    public void SetUI(TextMeshProUGUI timeText, TextMeshProUGUI dayText)
    {
        this.timeText = timeText;
        this.dayText = dayText;
    }
}
