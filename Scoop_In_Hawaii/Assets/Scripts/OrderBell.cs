using System.Collections;
using UnityEngine;

public class OrderBell : MonoBehaviour
{
    [SerializeField] GameObject tempImagePrefab;
    Transform tempPoint;

    GameObject currentSpawnedImage;

    float moveSpeed = 800f;

    private void Start()
    {
        GameObject pointObj = GameObject.Find("temp_Point");

        if (pointObj != null)
        {
            tempPoint = pointObj.transform;
        }
    }

    public void SpawnTemporaryImage()
    {
        if (currentSpawnedImage == null)
        {
            currentSpawnedImage = Instantiate(tempImagePrefab, tempPoint);
            RectTransform rt = currentSpawnedImage.GetComponent<RectTransform>();
            if (rt != null) rt.anchoredPosition = Vector2.zero;
            else currentSpawnedImage.transform.localPosition = Vector3.zero;

            Debug.Log("임시로 이미지 생성");
        }
        else
        {
            Debug.Log("이미 생성됨");
        }
    }

    public void ServeIceCream()
    {
        // ������ ���̽�ũ���� �ְ�, ��ǥ ������ ����Ǿ� ���� ���� ����
        if (currentSpawnedImage != null)
        {
            StartCoroutine(MoveIceCream());
        }
        else
        {
            Debug.LogWarning("아이스크림이 존재하지 않습니다.");
        }
    }
    private IEnumerator MoveIceCream()
    {
        Vector3 startPos = tempPoint.position;
        Vector3 targetPos = startPos + new Vector3(0, 1f, 0);

        float duration = 1.0f;
        float timer = 0f;

        while (timer < duration)
        {
            tempPoint.position = Vector3.Lerp(startPos, targetPos, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        tempPoint.position = targetPos;
        Debug.Log("이동 완료");
    }

}