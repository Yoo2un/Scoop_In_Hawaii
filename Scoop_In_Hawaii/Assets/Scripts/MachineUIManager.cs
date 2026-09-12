using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MachineUIManager : MonoBehaviour
{
    public GameObject conePanel;
    public GameObject barPanel;
    public GameObject machinePanel;
    public GameObject chocolatePanel;
    public GameObject vanillaPanel;


    public TMP_Text titleText;
    [SerializeField] private GameObject RepairPanel;
    [SerializeField] private GameObject RepairMiniGamePanel;

    [SerializeField] private RepairMiniGame repairMiniGame;
    [SerializeField] private RepairEffect repairEffect;

    [SerializeField] private BoxCollider2D showcaseCollider;

    public void OpenConeMachine()
    {  if (MachineModifier.Instance.ConeBroken)
        {
            RepairPanel.SetActive(true);

            if (showcaseCollider != null)
                showcaseCollider.enabled = false;

            return;
        }

        CloseAll();

        machinePanel.SetActive(true);
        conePanel.SetActive(true);
        chocolatePanel.SetActive(true);
        vanillaPanel.SetActive(true);

        titleText.text = "콘 아이스크림 제작";
        
        if (showcaseCollider != null)
            showcaseCollider.enabled = false;

    }

    public void OpenChocolate()
    {

        //chocolatePanel.SetActive(true);

        titleText.text = "초콜릿 스쿱";
    }

    public void OpenVanilla()
    {

        //vanillaPanel.SetActive(true);

        titleText.text = "바닐라 스쿱";
    }

    public void OpenBarMachine()
    {
        if (MachineModifier.Instance.BarBroken)
        {
            RepairPanel.SetActive(true);
            return;
        }

        CloseAll();

        machinePanel.SetActive(true);
        barPanel.SetActive(true);

        titleText.text = "바 아이스크림 제작";
    }

    public void OpenRepairMiniGame()
    {
        RepairPanel.SetActive(false);
        repairMiniGame.InitMiniGame();
        RepairMiniGamePanel.SetActive(true);
    }

    public void CallRepairMan()
    {
        RepairPanel.SetActive(false);

        StartCoroutine(repairEffect.PlayRepairEffect());
    }

    public void CloseAll()
    {
        conePanel.SetActive(false);
        barPanel.SetActive(false);
        machinePanel.SetActive(false);
        chocolatePanel.SetActive(false);
        vanillaPanel.SetActive(false);

        if (showcaseCollider != null)
            showcaseCollider.enabled = true;
    }

}
