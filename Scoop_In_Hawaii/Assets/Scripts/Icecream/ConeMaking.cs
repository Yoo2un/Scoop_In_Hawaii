using UnityEngine;

public class ConeMaking : MonoBehaviour
{
    private Cone currentCone;

    // 콘 제작 시작
    public void StartCone()
    {

        if (currentCone != null)
        {
            Debug.Log("이미 제작 중인 콘이 있습니다.");
            return;
        }

        currentCone = new Cone();
        currentCone.Type = IceCreamType.Cone;

        Debug.Log("콘 제작 시작");
    }


    [VisibleEnum(typeof(Flavor))]
    public void AddFlavor(int flavorIndex)
    {
        // 제작 중인 콘 확인
        if (currentCone == null)
        {
            Debug.Log("제작 중인 콘이 없습니다.");
            return;
        }

        // 토핑이 있으면 맛 추가 불가
        if (currentCone.Toppings.Count > 0)
        {
            Debug.Log("토핑이 이미 추가되어 있습니다.");
            return;
        }

        // 시럽이 있으면 맛 추가 불가
        if (currentCone.syrup != Syrup.None)
        {
            Debug.Log("시럽이 이미 추가되어 있습니다.");
            return;
        }

        // 최대 3가지 맛 제한
        if (currentCone.Flavors.Count >= 3)
        {
            Debug.Log("맛은 최대 3개까지 추가할 수 있습니다.");
            return;
        }

        // 맛 변환
        Flavor flavor = (Flavor)flavorIndex;

        // 재고 확인
        if (!InventoryManager.Instance.HasStock(flavor))
        {
            Debug.Log($"{flavor} 재고가 없습니다.");
            return;
        }

        // 맛 추가 및 재고 차감
        currentCone.Flavors.Add(flavor);
        InventoryManager.Instance.UseFlavor(flavor);

        Debug.Log($"맛 추가 성공: {flavor}");
        Debug.Log($"현재 콘의 맛 개수: {currentCone.Flavors.Count}");
        Debug.Log($"현재 콘의 맛 목록: {string.Join(", ", currentCone.Flavors)}");
    }

    [VisibleEnum(typeof(Topping))]
    public void AddTopping(int toppingIndex)
    {
        if (currentCone == null) return;

        Topping topping = (Topping)toppingIndex;

        if (currentCone.Toppings.Count >= 1)
        {
            Debug.Log("토핑은 하나만 추가할 수 있습니다.");
            return;
        }

        currentCone.Toppings.Add(topping);

        Debug.Log($"{topping} 토핑 추가");
    }

    [VisibleEnum(typeof(Syrup))]
    public void AddSyrup(int syrupIndex)
    {
        if (currentCone == null) return;

        if (currentCone.syrup != Syrup.None)
        {
            Debug.Log("시럽이 이미 있습니다.");
            return;
        }

        currentCone.syrup = (Syrup)syrupIndex;

        Debug.Log($"{currentCone.syrup} 시럽 추가");
    }

    public void CompleteCone()
    {
        if (currentCone == null)
        {
            Debug.Log("제작 중인 콘이 없습니다.");
            return;
        }

        if (currentCone.Flavors.Count == 0)
        {
            Debug.Log("맛을 먼저 추가해주세요.");
            return;
        }

        // IceCreamManager에 완성된 콘 전달
        IceCreamManager.Instance.CompleteIceCream(currentCone);

        // 현재 제작 데이터 초기화
        currentCone = null;

        Debug.Log("콘 제작 완료!");
    }
}
