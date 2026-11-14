using UnityEngine;

public abstract class IceCreamDisplay : MonoBehaviour
{
    public IceCream Data { get; private set; }

    public void Show(IceCream item)
    {
        Data = item;
        Draw(item);
    }

    protected abstract void Draw(IceCream item);

}
