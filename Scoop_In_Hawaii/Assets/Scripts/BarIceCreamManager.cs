using UnityEngine;

public class BarIceCreamManager : MonoBehaviour
{
    public static BarIceCreamManager Instance { get; private set; }

    [SerializeField]
    private GameObject barMakingPanel;

    private IceCream firstBar = null;

    private void Awake()
    {
        Instance = this;

        barMakingPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MakeFirstBar(string flavor)
    {
        if (firstBar != null)
        {
            return;
        }

        Flavor realFlavor;
        if ("Mango".Equals(flavor))
        {
            realFlavor = Flavor.Mango;
        } else if ("Soda".Equals(flavor)) {
            realFlavor = Flavor.Soda;
        } else {
            return;
        }

        firstBar = new IceCream();
        firstBar.Type = IceCreamType.Bar;
        firstBar.Flavors.Add(realFlavor);
    }
}
