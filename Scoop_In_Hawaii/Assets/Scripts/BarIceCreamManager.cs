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
    [SerializeField]
    private GameObject barMoldLeft;
    [SerializeField]
    private GameObject barMoldRight;
    private MoldSlot moldslotLeft;
    private MoldSlot moldslotRight;

    private IceCream firstBar = null;
    private IceCream secondBar = null;

    private FrozenTimer rightFrozenTimer;
    private FrozenTimer leftFrozenTimer;

    private void Awake()
    {
        Instance = this;

        panelCloseBtn.GetComponent<Button>().onClick.RemoveAllListeners();
        panelCloseBtn.GetComponent<Button>().onClick.AddListener(closeBarMakingPanel);

        barMachineBtn.GetComponent<Button>().onClick.RemoveAllListeners();
        barMachineBtn.GetComponent<Button>().onClick.AddListener(openBarMakingPanel);

        barMakingPanel.SetActive(false);

        leftFrozenTimer = GameObject.Find("LeftFrozenTimer").GetComponent<FrozenTimer>();
        rightFrozenTimer = GameObject.Find("RightFrozenTimer").GetComponent<FrozenTimer>();
        moldslotLeft = barMoldLeft.GetComponent<MoldSlot>();
        moldslotRight = barMoldRight.GetComponent<MoldSlot>();
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

        // ��� Ȯ��
        if (!InventoryManager.Instance.HasStock(realFlavor))
        {
            Debug.Log($"{realFlavor} ���� �����ϴ�.");
            return;
        }

        // ��� 1�� ���
        InventoryManager.Instance.UseFlavor(realFlavor);

        firstBar = new Bar();
        firstBar.Type = IceCreamType.Bar;
        firstBar.Flavors.Add(realFlavor);
        ((Bar)firstBar).status = Bar.Frozen.LIQUID;
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

        // ��� Ȯ��
        if (!InventoryManager.Instance.HasStock(realFlavor))
        {
            Debug.Log($"{realFlavor} ���� �����ϴ�.");
            return;
        }

        // ��� 1�� ���
        InventoryManager.Instance.UseFlavor(realFlavor);

        secondBar = new Bar();
        secondBar.Type = IceCreamType.Bar;
        secondBar.Flavors.Add(realFlavor);
        ((Bar)secondBar).status = Bar.Frozen.LIQUID;
    }

    public void ResetFirstBar()
    {
        firstBar = null;
    }

    public void ResetSecondBar()
    {
        secondBar = null;
    }

    public Bar TakeOutFirstBar()
    {
        if (firstBar == null) return null;

        Bar targetBar = (Bar)firstBar;
        ResetFirstBar();

        return targetBar;
    }
    public Bar TakeOutSecondBar()
    {
        if (secondBar == null) return null;

        Bar targetBar = (Bar)secondBar;
        ResetSecondBar();

        return targetBar;
    }

    public bool isFillFirstBar()
    {
        return firstBar != null;
    }

    public bool isFillSecondBar()
    {
        return secondBar != null;
    }

    public void LeftHeldBar()
    {
        if (((Bar)firstBar).status == Bar.Frozen.LIQUID)
        {
            ToastManager.Instance.Toast("아직 아이스크림이 얼지 않았습니다.");
            return;
        }

        TakeOutFirstBar();
        leftFrozenTimer.ResetTimer();
        moldslotLeft.ResetMold();
    }

    public void RightHeldBar()
    {
        if (((Bar)secondBar).status == Bar.Frozen.LIQUID)
        {
            ToastManager.Instance.Toast("아직 아이스크림이 얼지 않았습니다.");
            return;
        }

        TakeOutSecondBar();
        rightFrozenTimer.ResetTimer();
        moldslotRight.ResetMold();
    }

    public void SetFisrtBarStatus(Bar.Frozen status)
    {
        if (isFillFirstBar())
        {
            ((Bar)firstBar).status = status;
        }
    }

    public void SetSecondBarStatus(Bar.Frozen status)
    {
        if (isFillSecondBar())
        {
            ((Bar)secondBar).status = status;
        }
    }
}
