using System.Collections;
using UnityEngine;
using System.Linq;

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
        // 생성된 아이스크림이 있고, 목표 지점이 연결되어 있을 때만 실행
        if (currentSpawnedImage != null)
        {
            StartCoroutine(MoveIceCream());
        }
        else
        {
            //Debug.LogWarning("아이스크림이 존재하지 않습니다.");
        }

        if(gameManager.currentOrder.Equals(iceCreamMaking.currentIceCream))
        {
            //Debug.Log("완벽한 제작!");
        }
        else
        {
            if (gameManager.currentOrder.Type != iceCreamMaking.currentIceCream.Type)
            {
                //Debug.Log("종류가 다릅니다.");
            }

            if (!gameManager.currentOrder.Flavors.SequenceEqual(iceCreamMaking.currentIceCream.Flavors))
            {
                //Debug.Log("맛이 다릅니다.");
            }

            if (!gameManager.currentOrder.Toppings.SetEquals(iceCreamMaking.currentIceCream.Toppings))
            {
                //Debug.Log("토핑이 다릅니다.");
            }

            if (gameManager.currentOrder is Cone orderCone &&
               iceCreamMaking.currentIceCream is Cone madeCone)
            {
                if (orderCone.syrup != madeCone.syrup)
                {
                    //Debug.Log("시럽이 다릅니다.");
                }
            }
        }
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

        rt.anchoredPosition = targetPos;

        Debug.Log("이동 완료");
    }

}