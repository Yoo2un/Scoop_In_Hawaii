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

    // ������ �Ϸ�� ���̽�ũ���� ���޹޴� �Լ�
    public void CompleteIceCream(IceCream iceCream)
    {
        if (iceCream == null)
        {
            Debug.LogWarning("�ϼ��� ���̽�ũ���� �����ϴ�.");
            return;
        }

        // ���Կ� ���̽�ũ�� ���
        bool success = IceCreamSlots.Instance.Add(iceCream);

        if (!success)
        {
            Debug.LogWarning("���̽�ũ�� ���� ��� ����!");
            return;
        }

        Debug.Log($"���̽�ũ�� ���� �Ϸ�: {iceCream.Type}");
    }
}
