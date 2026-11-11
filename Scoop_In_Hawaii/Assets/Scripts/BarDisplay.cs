using UnityEngine;
using UnityEngine.UI;

public class BarDisplay : IceCreamDisplay
{
    [SerializeField] private Sprite soda;        // 이미지 : soda_all
    [SerializeField] private Sprite mango;       // 이미지 : mango_all

    protected override void Draw(IceCream item)
    {
        Image image = GetComponent<Image>();     // 같은 오브젝트에 있는 Image
        image.sprite = item.Flavors.Contains(Flavor.Soda) ? soda : mango;
    }
}
