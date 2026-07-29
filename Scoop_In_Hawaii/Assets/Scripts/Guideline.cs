using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;

public class Guideline : MonoBehaviour, IDragHandler, IEndDragHandler
{

    [System.Serializable]
    public struct GuidelineData
    {
        public Sprite guidelineSprite;
        public List<Vector3> pathPoints;
    }

    public GameObject machinePanel;
    public Image guidelineImage;    //가이드라인 이미지
    public LineRenderer userLine;   //사용자 그리기 라인

    [Header("가이드라인 이미지, 좌표")]
    public List<GuidelineData> guidelineDataList;

    private GuidelineData currentGuidelineData;     // 현재 선택된 가이드라인 데이터
    private List<Vector3> userPoints = new List<Vector3>();
    private bool isDrawing = true; // 중복 입력 방지용

    void OnEnable()
    {
        isDrawing = true;    // 입력 활성화
        userPoints.Clear();     //사용자 그리기 데이터 초기화
        userLine.positionCount = 0;    //사용자 라인 초기화

        // 랜덤으로 가이드라인 선택
        if (guidelineDataList != null && guidelineDataList.Count > 0)
        {
            int randomIndex = Random.Range(0, guidelineDataList.Count);
            currentGuidelineData = guidelineDataList[randomIndex];

            // 선택된 가이드라인 표시
            if (currentGuidelineData.guidelineSprite != null)
            {
                guidelineImage.sprite = currentGuidelineData.guidelineSprite;
                guidelineImage.gameObject.SetActive(true);
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform as RectTransform, eventData.position, eventData.pressEventCamera, out localPoint))
        {
            Vector3 newPoint = new Vector3(localPoint.x, localPoint.y, 0);

            if (userPoints.Count == 0 || Vector3.Distance(userPoints[userPoints.Count - 1], newPoint) > 2f)
            {
                userPoints.Add(newPoint);
                int count = userPoints.Count;
                userLine.positionCount = count;
                userLine.SetPosition(count - 1, userPoints[count - 1]);
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDrawing) return;

        // 드래그가 끝나면 그리기 잠금
        isDrawing = false;

        Debug.Log("그리기 완료");

        // 3. 가이드 라인 좌표값과  비교
        if (currentGuidelineData.pathPoints != null && currentGuidelineData.pathPoints.Count > 0)
        {
            float similarity = ComparePaths(currentGuidelineData.pathPoints, userPoints);
            Debug.Log($"유사도 오차 점수: {similarity}");
        }

        // 1초 후 패널 닫기
        StartCoroutine(ClosePanelAfterDelay(1.0f));
    }

    private float ComparePaths(List<Vector3> targetPath, List<Vector3> inputPath)
    {
        if (inputPath == null || inputPath.Count == 0) return float.MaxValue;

        float totalDistanceError = 0f;
        int sampleCount = 20;

        for (int i = 0; i < sampleCount; i++)
        {
            float t = (float)i / (sampleCount - 1);
            Vector3 targetPt = GetPointAlongPath(targetPath, t);
            Vector3 inputPt = GetPointAlongPath(inputPath, t);

            totalDistanceError += Vector3.Distance(targetPt, inputPt);
        }

        return totalDistanceError / sampleCount;
    }

    private Vector3 GetPointAlongPath(List<Vector3> path, float t)
    {
        if (path.Count == 1) return path[0];
        float totalLength = 0f;
        for (int i = 0; i < path.Count - 1; i++)
            totalLength += Vector3.Distance(path[i], path[i + 1]);

        float targetDist = totalLength * t;
        float currentDist = 0f;

        for (int i = 0; i < path.Count - 1; i++)
        {
            float segDist = Vector3.Distance(path[i], path[i + 1]);
            if (currentDist + segDist >= targetDist)
            {
                float localT = (targetDist - currentDist) / segDist;
                return Vector3.Lerp(path[i], path[i + 1], localT);
            }
            currentDist += segDist;
        }
        return path[path.Count - 1];
    }

    IEnumerator ClosePanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        machinePanel.SetActive(false);
    }
}