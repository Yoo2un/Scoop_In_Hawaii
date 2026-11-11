using UnityEngine;

public class OrderBell : MonoBehaviour
{
    private IceCreamMaking iceCreamMaking;

    private void Start()
    {
        iceCreamMaking = FindAnyObjectByType<IceCreamMaking>();
    }
    public void ServeIceCream()
    {
        float accuracy = CalculateAccuracy();

        int basePrice = GetBasePrice();

        int amount = Mathf.RoundToInt(basePrice * accuracy); //소수점 첫번째 자리에서 반올림

        iceCreamMaking.CompleteIceCream(amount);
        Debug.Log($"정확도 : {accuracy * 100}%");
        Debug.Log($"판매가 : {basePrice}");
        Debug.Log($"최종 금액 : {amount}");
    }

    private int GetBasePrice()
    {
        if (CustomerManager.Instance.currentOrder.Flavors.Count == 0)
            return 0;

        return IceCreamData.FlavorPrices[CustomerManager.Instance.currentOrder.Flavors[0]][0];
    }

    private float CalculateAccuracy()
    {
        float score = 0f;

        // 타입
        if (CustomerManager.Instance.currentOrder.Type == iceCreamMaking.currentIceCream.Type)
            score += 0.2f;

        //콘:시럽 포함 10%이기 때문에 0.5이고 바일 때는 시럽이 없기 때문에 0.6
        float flavorWeight = CustomerManager.Instance.currentOrder.Type == IceCreamType.Bar ? 0.6f : 0.5f;

        // 맛
        score += CalculateFlavorScore() * flavorWeight; // (2/3) * 0.5 = 0.33점

        // 토핑
        if (CustomerManager.Instance.currentOrder.Toppings.SetEquals(iceCreamMaking.currentIceCream.Toppings))
            score += 0.2f;

        // 시럽
        if (CustomerManager.Instance.currentOrder is Cone orderCone && iceCreamMaking.currentIceCream is Cone madeCone )
        {
            if (orderCone.syrup == madeCone.syrup)
            {
                score += 0.1f;
                Debug.Log("시럽 : " + (orderCone.syrup == madeCone.syrup));
            }
        }

        Debug.Log("===== 점수 =====");
        Debug.Log("타입 : " + (CustomerManager.Instance.currentOrder.Type == iceCreamMaking.currentIceCream.Type));

        Debug.Log("맛 : " + CalculateFlavorScore());

        Debug.Log("토핑 : " +
        CustomerManager.Instance.currentOrder.Toppings.SetEquals(iceCreamMaking.currentIceCream.Toppings));

        Debug.Log("최종 점수 : " + score);

        return score;
    }

    //여러 층 스쿱 비교
    private float CalculateFlavorScore()
    {
        Debug.Log(iceCreamMaking.currentIceCream);
        Debug.Log(iceCreamMaking.currentIceCream.Flavors.Count);

        int orderCount = CustomerManager.Instance.currentOrder.Flavors.Count;
        int madeCount = iceCreamMaking.currentIceCream.Flavors.Count;

        if (orderCount == 0)
            return madeCount == 0 ? 1f : 0f;

        int compareCount = Mathf.Min(orderCount, madeCount);

        int correct = 0;

        for (int i = 0; i < compareCount; i++)
        {
            if (CustomerManager.Instance.currentOrder.Flavors[i] ==
                iceCreamMaking.currentIceCream.Flavors[i])
            {
                correct++;
            }
        }

        return (float)correct / orderCount;
    }
}