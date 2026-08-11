using UnityEngine;
using UnityEngine.EventSystems;

public class ShowCaseClick : MonoBehaviour
{
    [SerializeField] private MachineUIManager machineUIManager;
    [SerializeField] private IceCreamMaking iceCreamMaking;

    private void OnMouseDown()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) // ui 요소가 있다면 실행하지 않음
        {
            return;
        }

        Debug.Log("쇼케이스 클릭!");

        machineUIManager.OpenConeMachine();
        iceCreamMaking.SelectType(0);
    }
}
