using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 재료 1개 사용
    public void UseFlavor(Flavor flavor)
    {
        if (IceCreamData.FlavorStocks[flavor] > 0)
        {
            IceCreamData.FlavorStocks[flavor]--;
            Debug.Log($"{flavor} 재고: {IceCreamData.FlavorStocks[flavor]}");
        }
    }

    public bool HasStock(Flavor flavor)
    {
        return IceCreamData.FlavorStocks[flavor] > 0;
    }
}
