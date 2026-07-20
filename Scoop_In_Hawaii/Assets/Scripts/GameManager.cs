using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    /// <summary>
    /// 게임 내에서 적용되는 모디파이어 종류
    /// </summary>
    public enum ModifierType { None, GoodEvent, BadEvent }
    public ModifierType modifier = ModifierType.None;

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
    /// 씬이 로드될 때 호출되는 이벤트를 등록한다.
    /// </summary>
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /// <summary>
    /// 등록된 씬 로드 이벤트를 해제하고
    /// 예약된 Invoke를 취소한다.
    /// </summary>
    void OnDisable()
    {
        CancelInvoke();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// 씬이 로드되면 게임을 초기화하고
    /// 씬에 맞는 동작을 수행한다.
    /// </summary>
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Equals("DayScene"))
        {
            UIManager.Instance.Initialize();
            UIManager.Instance.PlayIntro();

            DayManager.Instance.ResetDay();
            DayManager.Instance.StartTime();

            EconomyManager.Instance.profit = 0;

            UIManager.Instance.SetDayButton(DayStart, "장사 시작", new Color32(126, 255, 109, 255));

            Debug.Log($"하루 시작(현재 {DayManager.Instance.day}일차 아침)");
        }
        else if (scene.name.Equals("Result"))
        {
            ShowResult();
        }
    }

    /// <summary>
    /// 영업을 시작하고
    /// 손님 생성 및 게임 진행을 시작한다.
    /// </summary>
    public void DayStart()
    {
        if (DayManager.Instance.dayState == DayState.Morning)
        {
            UIManager.Instance.SetDayButton(CloseBusiness, "장사 종료", new Color32(212, 47, 41, 255));

            EconomyManager.Instance.profit = 0;
            DayManager.Instance.SetState(DayState.Open);

            Debug.Log("장사 시작");

            CustomerManager.Instance.StartBusiness();
        }
    }

    /// <summary>
    /// 영업을 종료하고
    /// 하루 종료 상태로 전환한다.
    /// </summary>
    public void CloseBusiness()
    {
        if (DayManager.Instance.dayState == DayState.Morning || DayManager.Instance.dayState == DayState.Open)
        {
            DayManager.Instance.SetState(DayState.Closed);

            CancelInvoke();
            DayManager.Instance.StopTime();
            StopAllCoroutines();

            CustomerManager.Instance.StopBusiness();

            Debug.Log("장사 종료");

            UIManager.Instance.SetDayButton(DayEnd, "하루 종료", new Color32(248, 224, 36, 255));
        }
    }

    /// <summary>
    /// 하루를 종료하고
    /// 결과 화면으로 이동한다.
    /// </summary>
    public void DayEnd()
    {
        if (DayManager.Instance.dayState == DayState.Closed)
        {
            DayManager.Instance.SetState(DayState.Result);

            CancelInvoke();
            DayManager.Instance.StopTime();
            StopAllCoroutines();

            DataPersistenceManager.instance.SaveGame();
            Debug.Log("하루 종료");
            SceneManager.LoadScene("Result");
        }
    }

    /// <summary>
    /// 하루의 영업 결과를 출력한다.
    /// </summary>
    public void ShowResult()
    {
        Debug.Log("장사 결과 정산");
    }

    /// <summary>
    /// 현재 설정된 모디파이어 효과를 실행한다.
    /// </summary>
    public void UseModifier()
    {
        Debug.Log($"useModifier() 호출됨. 현재 발동된 모디파이어: {modifier}");

        switch (modifier)
        {
            case ModifierType.GoodEvent:
                //SNSViralMdodifier.Instance.SNS_Viral();
                //TrendFlavorModifier.Instance.StartTrendFlavor();
                break;

            case ModifierType.BadEvent:
                MachineModifier.Instance.BreakMachine();
                //SeagullModifier.Instance.FlySeagull();
                break;
        }
    }
}