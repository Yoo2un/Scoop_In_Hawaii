using UnityEngine;
using UnityEngine.UI;

public class BarIceCreamManager : MonoBehaviour
{
    public static BarIceCreamManager Instance { get; private set; }
    public static bool ISOPENBARMAKINGPANEL = false;

    [SerializeField]
    private GameObject barMakingPanel;
    [SerializeField]
    private GameObject panelCloseBtn;
    [SerializeField]
    private GameObject barMachineBtn;

    private IceCream firstBar = null;
    private IceCream secondBar = null;

    private void Awake()
    {
        Instance = this;

        panelCloseBtn.GetComponent<Button>().onClick.RemoveAllListeners();
        panelCloseBtn.GetComponent<Button>().onClick.AddListener(closeBarMakingPanel);

        barMachineBtn.GetComponent<Button>().onClick.RemoveAllListeners();
        barMachineBtn.GetComponent<Button>().onClick.AddListener(openBarMakingPanel);

        barMakingPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void openBarMakingPanel()
    {
        if (!ISOPENBARMAKINGPANEL)
        {
            barMakingPanel.SetActive(true);
            ISOPENBARMAKINGPANEL = true;
        }
    }

    public void closeBarMakingPanel()
    {
        if (ISOPENBARMAKINGPANEL)
        {
            barMakingPanel.SetActive(false);
            ISOPENBARMAKINGPANEL = false;
        }
    }

    public void MakeFirstBar(string flavor)
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

        firstBar = new Bar();
        firstBar.Type = IceCreamType.Bar;
        firstBar.Flavors.Add(realFlavor);
        ((Bar)firstBar).status = Bar.Frozen.HALF;
    }

    public void MakeSecondBar(string flavor)
    {
        if (secondBar != null)
        {
            return;
        }

        Flavor realFlavor;
        if ("Mango".Equals(flavor))
        {
            realFlavor = Flavor.Mango;
        }
        else if ("Soda".Equals(flavor))
        {
            realFlavor = Flavor.Soda;
        }
        else
        {
            return;
        }

        secondBar = new Bar();
        secondBar.Type = IceCreamType.Bar;
        secondBar.Flavors.Add(realFlavor);
        ((Bar)secondBar).status = Bar.Frozen.HALF;
    }

    public void ResetFirstBar()
    {
        firstBar = null;
    }

    public void ResetSecondBar()
    {
        secondBar = null;
    }
}
