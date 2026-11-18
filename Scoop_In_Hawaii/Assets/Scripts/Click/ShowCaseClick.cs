
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowCaseClick : MonoBehaviour
{
    [SerializeField] private MachineUIManager machineUIManager;
    [SerializeField] private ConeMaking coneMaking;

    private void OnMouseDown()
    {
        if (EventSystem.current != null &&
            EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        machineUIManager.OpenConeMachine();
        coneMaking.StartCone();
    }
}
