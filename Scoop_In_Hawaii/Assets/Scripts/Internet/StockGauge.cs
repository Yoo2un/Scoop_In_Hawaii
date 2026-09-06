using UnityEngine;
using UnityEngine.UI;

public class StockGauge : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Flavor flavor;

    private void Start()
    {
        InventoryManager.Instance.OnStockChanged += OnStockChanged;

        UpdateGauge();
    }

    private void OnDestroy()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnStockChanged -= OnStockChanged;
        }
    }

    private void OnStockChanged(Flavor changedFlavor)
    {
        //Debug.Log($"StockGauge 이벤트 받음: {changedFlavor}, 내 맛: {flavor}");

        if (changedFlavor == flavor)
        {
            UpdateGauge();
        }
    }

    public void UpdateGauge()
    {
        int currentStock = IceCreamData.FlavorStocks[flavor];
        int maxStock = IceCreamData.MaxFlavorStocks[flavor];

        float ratio = (float)currentStock / maxStock;

        slider.value = ratio;

        Image fillImage = slider.fillRect.GetComponent<Image>();

        if (ratio > 0.5f)
        {
            fillImage.color = new Color(0.765f, 0.855f, 0.459f);
        }
        else if (ratio > 0.25f)
        {
            fillImage.color = Color.yellow;
        }
        else
        {
            fillImage.color = new Color(0.86f, 0.38f, 0.34f);
        }
    }
}