using UnityEngine;

public class IceCreamManager : MonoBehaviour
{
    public static IceCreamManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // 제작이 완료된 아이스크림을 전달받는 함수
    public void CompleteIceCream(IceCream iceCream)
    {
        if (iceCream == null)
        {
            Debug.LogWarning("완성된 아이스크림이 없습니다.");
            return;
        }

        // 슬롯에 아이스크림 등록
        bool success = IceCreamSlots.Instance.Add(iceCream);

        if (!success)
        {
            Debug.LogWarning("아이스크림 슬롯 등록 실패!");
            return;
        }

        Debug.Log($"아이스크림 제작 완료: {iceCream.Type}");
    }
}
