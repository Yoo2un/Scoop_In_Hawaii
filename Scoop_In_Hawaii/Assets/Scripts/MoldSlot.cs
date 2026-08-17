using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum MoldPosition
{
    Left,
    Right
}

public class MoldSlot : MonoBehaviour, IDropHandler
{
    public MoldPosition moldPosition;

    public bool isFilled = false;
    public LiquidType currentLiquid;

    [SerializeField]
    private GameObject filled_soda;
    [SerializeField]
    private GameObject filled_mango;
    [SerializeField]
    private FrozenTimer timer;

    private void Awake()
    {
        filled_mango.SetActive(false);
        filled_soda.SetActive(false);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            LiquidDraggable liquid = eventData.pointerDrag.GetComponent<LiquidDraggable>();

            if (liquid != null)
            {
                OnLiquidDropped(liquid.liquidType);
                timer.ResetTimer();
                timer.View();
            }
        }
    }


    // 용액이 드롭되었을 때 실행되는 함수
    public void OnLiquidDropped(LiquidType droppedLiquid)
    {
        if (isFilled)
        {
            Debug.Log($"이미 용액이 채워져 있습니다!");
            return;
        }

        isFilled = true;
        currentLiquid = droppedLiquid;

        OnFillSuccess(droppedLiquid);
    }

    private void OnFillSuccess(LiquidType liquid)
    {   
        if (liquid == LiquidType.Soda)
        {
            if (moldPosition == MoldPosition.Left)
            {
                BarIceCreamManager.Instance.MakeFirstBar("Soda");
            }
            else
            {
                BarIceCreamManager.Instance.MakeSecondBar("Soda");
            }
            filled_mango.SetActive(false);
            filled_soda.SetActive(true);
        }
        else if (liquid == LiquidType.Mango)
        {
            if (moldPosition == MoldPosition.Left)
            {
                BarIceCreamManager.Instance.MakeFirstBar("Mango");
            }
            else
            {
                BarIceCreamManager.Instance.MakeSecondBar("Mango");
            }
            filled_soda.SetActive(false);
            filled_mango.SetActive(true);
        }
    }

    // 몰드 리셋 함수 (얼린 후 다시 빈 상태로 만들 때 사용)
    public void ResetMold()
    {
        isFilled = false;
        filled_soda.SetActive(false);
        filled_mango.SetActive(false);

        if (moldPosition == MoldPosition.Left)
        {
            BarIceCreamManager.Instance.ResetFirstBar();
        }
        else
        {
            BarIceCreamManager.Instance.ResetSecondBar();
        }
    }
}