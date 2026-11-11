using UnityEngine;

public class IceCreamSlots : MonoBehaviour
{
    public static IceCreamSlots Instance { get; private set; }

    [SerializeField] private Transform[] slots;               // Slot_1~3
    [SerializeField] private IceCreamDisplay barDisplayPrefab;
    [SerializeField] private IceCreamDisplay coneDisplayPrefab;
    [SerializeField] private IceCreamMaking iceCreamMaking;
    [SerializeField] private MachineUIManager machineUI;

    private IceCreamDisplay[] displays;                       // 슬롯별 이미지, null이면 빈 슬롯

    private void Awake()
    {
        Instance = this;
        displays = new IceCreamDisplay[slots.Length];
    }

    private void Start()
    {
        BarIceCreamManager.Instance.OnBarTaken += OnBarTaken; // 바: 들기 신호
        machineUI.OnClosed += OnMachineClosed;                // 콘: 쇼케이스 닫힘 신호
    }

    private void OnDestroy()
    {
        if (BarIceCreamManager.Instance != null)
            BarIceCreamManager.Instance.OnBarTaken -= OnBarTaken;
        if (machineUI != null)
            machineUI.OnClosed -= OnMachineClosed;
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

    // 공용: 왼쪽부터 빈 슬롯에 이미지 생성
    public bool Add(IceCream item)
    {
        if (item == null) return false;

        for (int i = 0; i < slots.Length; i++)
        {
            if (displays[i] != null) continue;                // 이미지가 있으면 다음 슬롯

            IceCreamDisplay prefab = item is Bar ? barDisplayPrefab : coneDisplayPrefab;
            displays[i] = Instantiate(prefab, slots[i], false);
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

    private void OnMachineClosed()
    {
        Cone cone = iceCreamMaking.currentIceCream as Cone;
        if (cone == null || cone.Flavors.Count == 0) return;  // 맛을 안 골랐으면 무시
        if (Contains(cone)) return;                           // 이미 슬롯에 있는 콘이면 무시

        if (IsFull)
        {
            ToastManager.Instance.Toast("더 이상 들 수 없습니다.");
            return;
        }

        Add(cone);
    }

    private bool Contains(IceCream item)
    {
        foreach (IceCreamDisplay d in displays)
            if (d != null && d.Data == item) return true;
        return false;
    }
}