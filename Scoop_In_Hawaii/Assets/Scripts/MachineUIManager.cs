using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MachineUIManager : MonoBehaviour
{
    public GameObject conePanel;
    public GameObject barPanel;
    public GameObject machinePanel;

    public TMP_Text titleText;
    [SerializeField] private GameObject RepairPanel;
    [SerializeField] private GameObject RepairMiniGamePanel;

    [SerializeField] private RepairMiniGame repairMiniGame;
    [SerializeField] private RepairEffect repairEffect;


    public void OpenConeMachine()
    {
        if (MachineModifier.Instance.ConeBroken)
        {
            RepairPanel.SetActive(true);
            return;
        }

        CloseAll();

        machinePanel.SetActive(true);
        conePanel.SetActive(true);

        titleText.text = "콘 아이스크림 제작";
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
    }

}
