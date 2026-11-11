using UnityEngine;

public class IceCreamSlots : MonoBehaviour
{
    public static IceCreamSlots Instance { get; private set; }

    [SerializeField] private Transform[] slots;               // Slot_1~3
    [SerializeField] private IceCreamDisplay barDisplayPrefab;

    private IceCreamDisplay[] displays;                       // 배열로 생성, 리스트로 수정 필요

    private void Awake()
    {
        Instance = this;
        displays = new IceCreamDisplay[slots.Length];
    }

    private void Start()
    {
        BarIceCreamManager.Instance.OnBarTaken += OnBarTaken; // 바: 들기 신호
    }

    private void OnDestroy()
    {
        if (BarIceCreamManager.Instance != null)
            BarIceCreamManager.Instance.OnBarTaken -= OnBarTaken;
    }

    public bool IsFull
    {
        get
        {
            foreach (IceCreamDisplay d in displays)
                if (d == null) return false;
            return true;
        }
    }

    public bool Add(IceCream item)
    {
        if (item == null) return false;

        for (int i = 0; i < slots.Length; i++)
        {
            if (displays[i] != null) continue;

            displays[i] = Instantiate(barDisplayPrefab, slots[i], false);
            displays[i].Show(item);
            return true;
        }

        Debug.LogWarning("빈 슬롯이 없습니다.");
        return false;
    }

    private void OnBarTaken(Bar bar)
    {
        Add(bar);
    }
}