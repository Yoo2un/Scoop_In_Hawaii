using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MachineUIManager : MonoBehaviour
{
    public GameObject conePanel;
    public GameObject barPanel;
    public GameObject machinePanel;
    public GameObject chocolatePanel;
    public GameObject VanillaPanel;

    public TMP_Text titleText;

    public void OpenConeMachine()
    {
        CloseAll();

        machinePanel.SetActive(true);
        conePanel.SetActive(true);

        titleText.text = "콘 아이스크림 제작";
    }

    public void OpenChocolate()
    {

        chocolatePanel.SetActive(true);

        titleText.text = "초콜릿 스쿱";
    }

    public void OpenVanilla()
    {

        VanillaPanel.SetActive(true);

        titleText.text = "바닐라 스쿱";
    }

    public void OpenBarMachine()
    {
        CloseAll();

        machinePanel.SetActive(true);
        barPanel.SetActive(true);

        titleText.text = "바 아이스크림 제작";
    }

    public void CloseAll()
    {
        conePanel.SetActive(false);
        barPanel.SetActive(false);
        machinePanel.SetActive(false);
        chocolatePanel.SetActive(false);
    }
}
