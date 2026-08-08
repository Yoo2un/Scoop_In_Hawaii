using System.Collections;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class FrozenTimer : MonoBehaviour
{
    [SerializeField]
    private MoldPosition moldPosition;
    [SerializeField]
    private GameObject timerStartBtnObj;
    [SerializeField]
    private TextMeshProUGUI timerStartText;

    [SerializeField]
    private Slider bigSlider;
    [SerializeField]
    private GameObject bigRedSliderHandle;

    [SerializeField]
    private Slider smallSlider;
    [SerializeField]
    private GameObject smallRedSliderHandle;

    [SerializeField]
    private GameObject frozenUI;

    public int timerCount = 0;
    public Bar.Frozen currentState = Bar.Frozen.HALF;
    public bool isRunning = false;
    private Button timerStartBtn;
    private Image timerStartImage;

    private Coroutine timerCoroutine;

    private void Awake()
    {
        timerStartBtn = timerStartBtnObj.GetComponent<Button>();
        timerStartImage = timerStartBtnObj.GetComponent<Image>();

        timerStartImage.color = new Color32(77, 250, 240, 255);
        timerStartText.text = "얼리기 시작";
        timerStartBtn.onClick.RemoveAllListeners();
        timerStartBtn.onClick.AddListener(StartTimer);

        bigRedSliderHandle.SetActive(false);
        smallRedSliderHandle.SetActive(false);
        bigSlider.value = 0;
        smallSlider.value = 0;

        frozenUI.SetActive(false);
    }

    public void StartTimer()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }
        timerCoroutine = StartCoroutine(TimerRoutine());

        timerStartImage.color = new Color32(250, 160, 77, 255);
        timerStartText.text = "얼리기 종료";
        timerStartBtn.onClick.RemoveAllListeners();
        timerStartBtn.onClick.AddListener(StopTimer);
    }

    public void StopTimer()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }
        isRunning = false;

        timerStartImage.color = new Color32(77, 250, 240, 255);
        timerStartText.text = "얼리기 시작";
        timerStartBtn.onClick.RemoveAllListeners();
        timerStartBtn.onClick.AddListener(StartTimer);
    }

    public void ResetTimer()
    {
        StopTimer();
        timerCount = 0;
        currentState = Bar.Frozen.HALF;

        timerStartImage.color = new Color32(77, 250, 240, 255);
        timerStartText.text = "얼리기 시작";
        timerStartBtn.onClick.RemoveAllListeners();
        timerStartBtn.onClick.AddListener(StartTimer);

        bigSlider.value = 0;
        smallSlider.value = 0;
        bigRedSliderHandle.SetActive(false);
        bigSlider.handleRect.GetComponent<Image>().enabled = true;
        smallRedSliderHandle.SetActive(false);
        smallSlider.handleRect.GetComponent<Image>().enabled = true;

        frozenUI.SetActive(false);
    }

    private IEnumerator TimerRoutine()
    {
        isRunning = true;
        WaitForSeconds waitPointOne = new WaitForSeconds(0.1f);

        while (timerCount < 90)
        {
            UpdateState(timerCount);
            yield return waitPointOne;
            timerCount++;
            bigSlider.value = (float)timerCount / 90f;
            smallSlider.value = (float)timerCount / 90f;
        }

        // 90 카운트(9.0초) 도달 시 DEEP 상태로 고정 후 종료
        timerCount = 90;
        currentState = Bar.Frozen.DEEP;
        isRunning = false;
        Debug.Log($"[{moldPosition}] State: DEEP || Timer Stop");


        bigRedSliderHandle.SetActive(true);
        bigSlider.handleRect.GetComponent<Image>().enabled = false;
        smallRedSliderHandle.SetActive(true);
        smallSlider.handleRect.GetComponent<Image>().enabled = false;
    }

    private void UpdateState(int count)
    {
        if (count >= 0 && count <= 59)
        {
            currentState = Bar.Frozen.HALF;
        }
        else if (count >= 60 && count <= 89)
        {
            currentState = Bar.Frozen.PERFECT;
        }
        else if (count >= 90)
        {
            currentState = Bar.Frozen.DEEP;
        }
        else
        {
            currentState = Bar.Frozen.HALF;
        }

        Debug.Log($"[{moldPosition}] Count: {count} ({count * 0.1f:F1}s) | State: {currentState}");
    }

    public void View()
    {
        frozenUI.SetActive(true);
    }
}