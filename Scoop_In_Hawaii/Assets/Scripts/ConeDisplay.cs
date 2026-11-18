using UnityEngine;
using UnityEngine.UI;

public class ConeDisplay : IceCreamDisplay
{
    [Header("UI")]
    [SerializeField] private RectTransform scoopContainer;
    [SerializeField] private Image scoopPrefab;

    [Header("Flavor Sprites")]
    [SerializeField] private Sprite vanillaSprite;
    [SerializeField] private Sprite chocolateSprite;

    [Header("Layout")]
    [SerializeField] private float scoopSpacing = 25f;

    protected override void Draw(IceCream item)
    {
        // ±âÁ¸ ½ºÄò Á¦°Å
        foreach (Transform child in scoopContainer)
        {
            Destroy(child.gameObject);
        }

        // ¸À¿¡ µû¶ó ½ºÄò »ý¼º
        for (int i = 0; i < item.Flavors.Count; i++)
        {
            Image scoop = Instantiate(scoopPrefab, scoopContainer);

            scoop.sprite = GetFlavorSprite(item.Flavors[i]);

            RectTransform rect = scoop.GetComponent<RectTransform>();
            rect.anchoredPosition = Vector2.zero;
        }
    }

    private Sprite GetFlavorSprite(Flavor flavor)
    {
        switch (flavor)
        {
            case Flavor.Vanilla:
                return vanillaSprite;

            case Flavor.Chocolate:
                return chocolateSprite;

            default:
                return null;
        }
    }
}