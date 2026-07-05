using System.Collections;
using UnityEngine;
using System.Linq;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

public class OrderBell : MonoBehaviour
{
    [SerializeField] GameObject tempImagePrefab;
    Transform tempPoint;

    GameObject currentSpawnedImage;
    
    private GameManager gameManager;
    private IceCreamMaking iceCreamMaking;

    //float moveSpeed = 800f;

    private void Start()
    {
        GameObject pointObj = GameObject.Find("temp_Point");
        gameManager = FindAnyObjectByType<GameManager>();
        iceCreamMaking = FindAnyObjectByType<IceCreamMaking>();

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
            //Debug.LogWarning("���̽�ũ���� �������� �ʽ��ϴ�.");
        }

        float accuracy = CalculateAccuracy();

        int basePrice = GetBasePrice();

        int amount = Mathf.RoundToInt(basePrice * accuracy); //소수점 첫번째 자리에서 반올림

        iceCreamMaking.CompleteIceCream(amount);
        Debug.Log($"정확도 : {accuracy * 100}%");
        Debug.Log($"판매가 : {basePrice}");
        Debug.Log($"최종 금액 : {amount}");
    }
    private IEnumerator MoveIceCream()
    {
        RectTransform rt = currentSpawnedImage.GetComponent<RectTransform>();

        Vector2 startPos = rt.anchoredPosition;
        Vector2 targetPos = startPos + new Vector2(0, 200);

        float duration = 1f;
        float timer = 0f;

        while (timer < duration)
        {
            rt.anchoredPosition = Vector2.Lerp(startPos, targetPos, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        tempPoint.position = targetPos;
        Debug.Log("이동 완료");
    }

    private int GetBasePrice()
    {
        Flavor flavor = GameManager.Instance.currentOrder.Flavors[0];

        return IceCreamData.FlavorPrices[flavor];
    }

    private float CalculateAccuracy()
    {
        float score = 0f;

        // 타입
        if (GameManager.Instance.currentOrder.Type == iceCreamMaking.currentIceCream.Type)
            score += 0.2f;

        // 맛
        if (GameManager.Instance.currentOrder.Flavors.SequenceEqual(iceCreamMaking.currentIceCream.Flavors))
            score += 0.5f;

        // 토핑
        if (GameManager.Instance.currentOrder.Toppings.SetEquals(iceCreamMaking.currentIceCream.Toppings))
            score += 0.2f;

        // 시럽
        if (GameManager.Instance.currentOrder is Cone orderCone && iceCreamMaking.currentIceCream is Cone madeCone && orderCone.syrup == madeCone.syrup)
        {
            score += 0.1f;
        }

        return score;
    }
}