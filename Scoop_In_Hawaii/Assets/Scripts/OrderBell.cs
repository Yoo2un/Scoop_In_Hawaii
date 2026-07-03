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

        if(gameManager.currentOrder.Equals(iceCreamMaking.currentIceCream))
        {
            //Debug.Log("�Ϻ��� ����!");
        }
        else
        {
            if (gameManager.currentOrder.Type != iceCreamMaking.currentIceCream.Type)
            {
                //Debug.Log("������ �ٸ��ϴ�.");
            }

            if (!gameManager.currentOrder.Flavors.SequenceEqual(iceCreamMaking.currentIceCream.Flavors))
            {
                //Debug.Log("���� �ٸ��ϴ�.");
            }

            if (!gameManager.currentOrder.Toppings.SetEquals(iceCreamMaking.currentIceCream.Toppings))
            {
                //Debug.Log("������ �ٸ��ϴ�.");
            }

            if (gameManager.currentOrder is Cone orderCone &&
               iceCreamMaking.currentIceCream is Cone madeCone)
            {
                if (orderCone.syrup != madeCone.syrup)
                {
                    //Debug.Log("�÷��� �ٸ��ϴ�.");
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

        tempPoint.position = targetPos;
        Debug.Log("이동 완료");
    }

}