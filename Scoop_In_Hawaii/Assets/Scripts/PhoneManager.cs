using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

public class PhoneManager : MonoBehaviour
{
    public RectTransform phone;

    private bool isUp = false;

    public Vector2 downPosition = new Vector2(800, -700); 
    public Vector2 upPosition = new Vector2(800, -275);
    public float duration = 0.2f; // 이동 시간

    private Coroutine moveCoroutine;

    public void TogglePhone()
    {
        Vector2 targetPos = isUp ? downPosition : upPosition;

        // 이미 움직이고 있던 중이면 이전 계산을 취소
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);

        // 움직임 계산 시작
        moveCoroutine = StartCoroutine(Co_MovePhone(targetPos));

        isUp = !isUp;
    }

    // 움직일 때만 작동하는 특수 함수(코루틴)
    IEnumerator Co_MovePhone(Vector2 targetPos)
    {
        Vector2 startPos = phone.anchoredPosition;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            float percent = time / duration;

            // 정해진 시간 동안 부드럽게 이동
            phone.anchoredPosition = Vector2.Lerp(startPos, targetPos, percent);
            yield return null; // 다음 프레임까지 대기
        }

        phone.anchoredPosition = targetPos; // 목표치에 고정
        moveCoroutine = null; // 연산 종료
    }
}
