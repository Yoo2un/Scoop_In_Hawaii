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

    public LiquidType currentLiquid;

    [SerializeField]
    private GameObject image_stick;
    [SerializeField]
    private GameObject filled_soda;
    [SerializeField]
    private GameObject filled_mango;
    [SerializeField]
    private GameObject half_soda;
    [SerializeField]
    private GameObject half_mango;
    [SerializeField]
    private FrozenTimer timer;
    private bool stick = false;

    private void Awake()
    {
        image_stick.SetActive(false);
        filled_mango.SetActive(false);
        filled_soda.SetActive(false);
        half_soda.SetActive(false);
        half_mango.SetActive(false);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            LiquidDraggable liquid = eventData.pointerDrag.GetComponent<LiquidDraggable>();
            StickDraggable stick = eventData.pointerDrag.GetComponent<StickDraggable>();

            if (liquid != null)
            {
                OnLiquidDropped(liquid.liquidType);
            } else if (stick != null)
            {
                OnStickDropped();
            }
        }
    }


    // 용액이 드롭되었을 때 실행되는 함수
    public void OnLiquidDropped(LiquidType droppedLiquid)
    {
        if (!stick)
        {
            ToastManager.Instance.Toast("막대를 먼저 넣어주세요.");
            Debug.Log($"막대를 먼저 넣어주세요.");
            return;
        }

        if (isFilled())
        {
            ToastManager.Instance.Toast("이미 용액이 채워져 있습니다!");
            Debug.Log($"이미 용액이 채워져 있습니다!");
            return;
        }

        currentLiquid = droppedLiquid;

        OnFillSuccess(droppedLiquid);
    }

    public void OnStickDropped()
    {
        if (stick)
        {
            ToastManager.Instance.Toast("이미 막대가 넣어져 있습니다!");
            Debug.Log($"이미 막대가 넣어져 있습니다!");
            return;
        }

        stick = true;
        image_stick.SetActive(true);
    }

    private void OnFillSuccess(LiquidType liquid)
    {
        timer.ResetTimer();
        timer.View();
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
            image_stick.SetActive(false);
            filled_mango.SetActive(false);
            filled_soda.SetActive(true);
            half_soda.SetActive(false);
            half_mango.SetActive(false);
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
            image_stick.SetActive(false);
            filled_soda.SetActive(false);
            filled_mango.SetActive(true);
            half_soda.SetActive(false);
            half_mango.SetActive(false);
        }
    }

    // 몰드 리셋 함수 (얼린 후 다시 빈 상태로 만들 때 사용)
    public void ResetMold()
    {
        stick = false;
        image_stick.SetActive(false);
        filled_soda.SetActive(false);
        filled_mango.SetActive(false);
        half_soda.SetActive(false);
        half_mango.SetActive(false);

        if (moldPosition == MoldPosition.Left)
        {
            BarIceCreamManager.Instance.ResetFirstBar();
        }
        else
        {
            BarIceCreamManager.Instance.ResetSecondBar();
        }
    }

    public bool isFilled()
    {
        if (moldPosition == MoldPosition.Left)
        {
            return BarIceCreamManager.Instance.isFillFirstBar();
        }
        else
        {
            return BarIceCreamManager.Instance.isFillSecondBar();
        }
    }

    public void FrozenEvent1()
    {
        if (filled_soda.activeSelf) 
        {
            filled_soda.SetActive(false);
            half_soda.SetActive(true);
        }
        else 
        {
            filled_mango.SetActive(false);
            half_mango.SetActive(true);
        }
    }
}