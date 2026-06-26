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
    MachineModifier machineModifier;

    private void Start()
    {
        machineModifier = FindAnyObjectByType<MachineModifier>();
    }

    public void OpenConeMachine()
    {
        if (machineModifier.ConeBroken)
        {
            RepairPanel.SetActive(true);
            return;
        }

        CloseAll();

        machinePanel.SetActive(true);
        conePanel.SetActive(true);

        titleText.text = "콘 제작";
    }

    public void OpenBarMachine()
    {
        if (machineModifier.BarBroken)
        {
            RepairPanel.SetActive(true);
            return;
        }

        CloseAll();

        machinePanel.SetActive(true);
        barPanel.SetActive(true);

        titleText.text = "바 제작";
    }

    public void OpenRepairMiniGame()
    {
        RepairPanel.SetActive(false);
        RepairMiniGamePanel.SetActive(true);
    }

    public void CloseAll()
    {
       machinePanel.SetActive(false);
    }

}
