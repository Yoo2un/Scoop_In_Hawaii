using System.Collections.Generic;
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

    // 균등 분포 알고리즘용 빈도수 저장 테이블
    private int[] modifierFrequency;

    public ModifierData[] modifierArray;

    private void Start()
    {
        ResetModifierTimer();
        modifierFrequency = new int[modifierArray.Length];
    }

    private void Update()
    {
        if (GameManager.Instance == null || DayManager.Instance == null || (DayManager.Instance.dayState != DayState.Open &&
            DayManager.Instance.dayState != DayState.Morning))
            return;

        // 1. 전체 장사 시간 타이머 (4분 제한)
        currentDayTimer += Time.deltaTime;
        if (currentDayTimer >= totalDayTime)
        {
            currentDayTimer = 0f;
            Debug.Log("4분이 경과하여 장사를 강제 종료합니다.");
            GameManager.Instance.CloseBusiness();
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

        ModifierData selectedModifier = null;

        // 랜덤 기반 보정 시스템 (연속성 체크)
        //if (continuousBadCount >= MaxContinuousStreak)
        //{
        //    Debug.Log("[보정] 나쁜 이벤트가 연속으로 나와 좋은 이벤트를 강제 배정합니다.");
        //    selectedModifier = ChooseGoodModifierWithBalancedDistribution();
        //}
        //else if (continuousGoodCount >= MaxContinuousStreak)
        //{
        //    Debug.Log("[보정] 좋은 이벤트가 연속으로 나와 나쁜 이벤트를 강제 배정합니다.");
        //    selectedModifier = ChooseBadModifierWithBalancedDistribution();
        //}
        //else
        //{
        //    // 균등 분포 알고리즘 (두 개를 뽑아 빈도가 낮은 쪽 선택)
        //    selectedModifier = ChooseModifierWithBalancedDistribution();
        //}

        // 일회성 / 다회성 구분 (주석 처리)
        /*
        if (IsOneTimeModifier(selectedModifier) && HasAlreadyTriggered(selectedModifier))
        {
            Debug.Log("일회성 모디파이어가 이미 발동했으므로 스킵합니다.");
            return;
        }
        */

        // 최종 결정된 모디파이어 상태 반영 및 연속성 카운트 업데이트
        if (selectedModifier == null) { }
        else if (selectedModifier.modifierType == ModifierType.GoodEvent)
        {
            continuousGoodCount++;
            continuousBadCount = 0;
        }
        else if (selectedModifier.modifierType == ModifierType.BadEvent)
        {
            continuousBadCount++;
            continuousGoodCount = 0;
        }
        else if (selectedModifier.modifierType == ModifierType.NeutralEvent)
        {
            continuousGoodCount = 0;
            continuousBadCount = 0;
        }

        // 테스트용
        selectedModifier = modifierArray[4];

        // 매니저에 전달 및 실행
        GameManager.Instance.modifier = selectedModifier;
        GameManager.Instance.UseModifier();
    }

    // 2개를 뽑아서 빈도가 작은 것을 선택하는 균등 분포 함수
    private ModifierData ChooseModifierWithBalancedDistribution()
    {
        // 2개의 후보를 독립적으로 랜덤 픽
        int candidateA = Random.Range(0, modifierArray.Length);
        int candidateB = Random.Range(0, modifierArray.Length);

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
        modifierFrequency[chosenIndex]++;

        return modifierArray[chosenIndex];
    }

    private ModifierData ChooseGoodModifierWithBalancedDistribution()
    {
        // 2개의 후보를 독립적으로 랜덤 픽
        List<(ModifierData data, int index)> goodEvents = new List<(ModifierData data, int index)>();
        for (int i = 0; i < modifierArray.Length; i++)
        {
            if (modifierArray[i].modifierType == ModifierType.GoodEvent)
            {
                goodEvents.Add((modifierArray[i], i));
            }
        }
        if (goodEvents.Count <= 0)
        {
            return null;
        }

        int candidateA = Random.Range(0, goodEvents.Count);
        int candidateB = Random.Range(0, goodEvents.Count);

        int chosenIndex;

        // 두 후보 중 누적 빈도수가 더 적은 쪽을 선택
        if (modifierFrequency[goodEvents[candidateA].index] <= modifierFrequency[goodEvents[candidateB].index])
        {
            chosenIndex = goodEvents[candidateA].index;
        }
        else
        {
            chosenIndex = goodEvents[candidateB].index;
        }
        modifierFrequency[chosenIndex]++;

        return modifierArray[chosenIndex];
    }

    private ModifierData ChooseBadModifierWithBalancedDistribution()
    {
        // 2개의 후보를 독립적으로 랜덤 픽
        List<(ModifierData data, int index)> badEvents = new List<(ModifierData data, int index)>();
        for (int i = 0; i < modifierArray.Length; i++)
        {
            if (modifierArray[i].modifierType == ModifierType.BadEvent)
            {
                badEvents.Add((modifierArray[i], i));
            }
        }
        if (badEvents.Count <= 0)
        {
            return null;
        }

        int candidateA = Random.Range(0, badEvents.Count);
        int candidateB = Random.Range(0, badEvents.Count);

        int chosenIndex;

        // 두 후보 중 누적 빈도수가 더 적은 쪽을 선택
        if (modifierFrequency[badEvents[candidateA].index] <= modifierFrequency[badEvents[candidateB].index])
        {
            chosenIndex = badEvents[candidateA].index;
        }
        else
        {
            chosenIndex = badEvents[candidateB].index;
        }
        modifierFrequency[chosenIndex]++;

        return modifierArray[chosenIndex];
    }
}
