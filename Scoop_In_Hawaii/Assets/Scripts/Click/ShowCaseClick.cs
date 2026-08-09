using UnityEngine;

public class ShowCaseClick : MonoBehaviour
{
    [SerializeField] private MachineUIManager machineUIManager;
    [SerializeField] private IceCreamMaking iceCreamMaking;

    private void OnMouseDown()
    {
        Debug.Log("쇼케이스 클릭!");

        machineUIManager.OpenConeMachine();
        iceCreamMaking.SelectType(0);
    }
}
