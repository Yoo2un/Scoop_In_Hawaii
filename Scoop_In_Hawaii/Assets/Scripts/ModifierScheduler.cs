using UnityEngine;

public class ModifierScheduler : MonoBehaviour
{
    // 4분 == 240초
    private float totalDayTime = 240f;
    private float currentDayTimer = 0f;

    private float targetModifierTime;
    private float modifierTimer = 0f;

    private int continuousGoodCount = 0;
    private int continuousBadCount = 0;
    // 연속 카운트 제한 제한
    private const int MaxContinuousStreak = 2;

    // 균등 분포 알고리즘용 빈도수 저장 테이블 (0: Good, 1: Bad 가정)
    private int[] modifierFrequency = new int[2];

    private void Start()
    {
        ResetModifierTimer();
    }

    private void Update()
    {
        // 장사 중(Open)일 때만 타이머 가동
        if (GameManager.Instance == null || GameManager.Instance.dayState != DayState.Open)
            return;

        // 1. 전체 장사 시간 타이머 (4분 제한)
        currentDayTimer += Time.deltaTime;
        if (currentDayTimer >= totalDayTime)
        {
            currentDayTimer = 0f;
            Debug.Log("4분이 경과하여 장사를 강제 종료합니다.");
            GameManager.Instance.DayEnd();
            return;
        }

        // 2. 모디파이어 스케줄러 타이머 (30~60초 랜덤)
        modifierTimer += Time.deltaTime;
        if (modifierTimer >= targetModifierTime)
        {
            EvaluateAndTriggerModifier();
            ResetModifierTimer();
        }
    }

    private void ResetModifierTimer()
    {
        modifierTimer = 0f;
        targetModifierTime = Random.Range(30f, 60f);
        Debug.Log($"다음 모디파이어 생성까지 걸리는 시간 : {targetModifierTime:F1}초");
    }

    private void EvaluateAndTriggerModifier()
    {
        Debug.Log("[Scheduler] 모디파이어 조건 평가 시작...");

        /* // 3일에 한 번 나오기 조건 (주석 처리)
        if (GameManager.Instance.day % 3 != 0)
        {
            Debug.Log("오늘은 모디파이어가 안 나오는 날입니다. (3일 주기 조건)");
            return;
        }
        */

        GameManager.ModifierType selectedModifier = GameManager.ModifierType.None;

        // 랜덤 기반 보정 시스템 (연속성 체크)
        if (continuousBadCount >= MaxContinuousStreak)
        {
            Debug.Log("[보정] 나쁜 이벤트가 연속으로 나와 좋은 이벤트를 강제 배정합니다.");
            selectedModifier = GameManager.ModifierType.GoodEvent;
        }
        else if (continuousGoodCount >= MaxContinuousStreak)
        {
            Debug.Log("[보정] 좋은 이벤트가 연속으로 나와 나쁜 이벤트를 강제 배정합니다.");
            selectedModifier = GameManager.ModifierType.BadEvent;
        }
        else
        {
            // 균등 분포 알고리즘 (두 개를 뽑아 빈도가 낮은 쪽 선택)
            selectedModifier = ChooseModifierWithBalancedDistribution();
        }

        // 일회성 / 다회성 구분 (주석 처리)
        /*
        if (IsOneTimeModifier(selectedModifier) && HasAlreadyTriggered(selectedModifier))
        {
            Debug.Log("일회성 모디파이어가 이미 발동했으므로 스킵합니다.");
            return;
        }
        */

        // 최종 결정된 모디파이어 상태 반영 및 연속성 카운트 업데이트
        if (selectedModifier == GameManager.ModifierType.GoodEvent)
        {
            continuousGoodCount++;
            continuousBadCount = 0;
            modifierFrequency[0]++; // 빈도 증가
        }
        else if (selectedModifier == GameManager.ModifierType.BadEvent)
        {
            continuousBadCount++;
            continuousGoodCount = 0;
            modifierFrequency[1]++; // 빈도 증가
        }

        // 매니저에 전달 및 실행
        GameManager.Instance.modifier = selectedModifier;
        GameManager.Instance.UseModifier();
    }

    // 2개를 뽑아서 빈도가 작은 것을 선택하는 균등 분포 함수
    private GameManager.ModifierType ChooseModifierWithBalancedDistribution()
    {
        // 2개의 후보를 독립적으로 랜덤 픽 (0: Good, 1: Bad)
        int candidateA = Random.Range(0, 2);
        int candidateB = Random.Range(0, 2);

        int chosenIndex;

        // 두 후보 중 누적 빈도수가 더 적은 쪽을 선택
        if (modifierFrequency[candidateA] <= modifierFrequency[candidateB])
        {
            chosenIndex = candidateA;
        }
        else
        {
            chosenIndex = candidateB;
        }

        return chosenIndex == 0 ? GameManager.ModifierType.GoodEvent : GameManager.ModifierType.BadEvent;
    }
}
