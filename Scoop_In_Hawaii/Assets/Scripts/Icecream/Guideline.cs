using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;

public class Guideline : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public GameObject machinePanel;

    public LineRenderer guideLine;
    public LineRenderer userLine;
    public List<List<Vector3>> guidelinePaths; // 가이드라인

    private List<Vector3> currentPath;
    private List<Vector3> userPoints = new List<Vector3>();

    void OnEnable()
    {
        // 1. 패널 활성화 시 랜덤 경로 선택


        // 2. 그리기 데이터 초기화

        userPoints.Clear();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform as RectTransform, eventData.position, eventData.pressEventCamera, out localPoint);

        // 사용자 드래그경로 저장
        userPoints.Add(new Vector3(localPoint.x, localPoint.y, 0));
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("스쿱 완료");
        StartCoroutine(ClosePanelAfterDelay(1.0f));
    }

    IEnumerator ClosePanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        machinePanel.SetActive(false);
    }
}