using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MachineUIManager : MonoBehaviour
{
    public GameObject conePanel;
    public GameObject barPanel;
    public GameObject machinePanel;

    public TMP_Text titleText;

    public void OpenConeMachine()
    {
        CloseAll();

        machinePanel.SetActive(true);
        conePanel.SetActive(true);

        titleText.text = "콘 제작";
    }

    public void OpenBarMachine()
    {
        CloseAll();

        machinePanel.SetActive(true);
        barPanel.SetActive(true);

        titleText.text = "바 제작";
    }

    public void CloseAll()
    {
       machinePanel.SetActive(false);
    }
}
