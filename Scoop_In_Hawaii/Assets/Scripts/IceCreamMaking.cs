using TMPro;
using UnityEngine;

public class IceCreamMaking : MonoBehaviour, IDataPersistence
{
    public IceCream currentIceCream;
    public TMP_Text moneyText;
    private int money = 0;

    public void LoadData(GameData data)
    {
        this.money = data.money;
        moneyText.text = money.ToString();
    }

    public void SaveData(ref GameData data)
    {
        data.money = this.money;
    }
using UnityEngine;

public class IceCreamMaking : MonoBehaviour
{
    public IceCream currentIceCream;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }

    [VisibleEnum(typeof(IceCreamType))]
    public void SelectType(int type)
    {
        if (type == 0) //���̸�
        {
            currentIceCream = new Cone();
        }
        else //���̸�
        {
            currentIceCream = new IceCream();
        }
        currentIceCream.Type = (IceCreamType)type;

        Debug.Log(currentIceCream.Type + "�߰�");
    }


    [VisibleEnum(typeof(Flavor))]
    public void AddFlavor(int flavorIndex)
    {
        if (currentIceCream == null)
        {
            return;
        }

        // ������ �̹� �ö��ִ��� �˻�
        if(currentIceCream.Toppings.Count > 0)
        {
            Debug.Log("�̹� ������ �ö󰡼� ���� �߰��� �� �����ϴ�");
            return;
        }

        // �÷��� �̹� �ö��ִ��� �˻�
        if(currentIceCream.Type == IceCreamType.Cone)
        {
            Cone cone = (Cone)currentIceCream;

            if (cone.syrup  != Syrup.None)
            {
                Debug.Log("�̹� �÷��� �ö󰡼� ���� �߰��� �� �����ϴ�");
                return;
            }
        }

        Flavor flavor = (Flavor)flavorIndex;

        // ���� Ÿ�Կ��� ��� ������ ������ �˻�
        if (!IceCreamData.TypeFlavors[currentIceCream.Type].Contains(flavor))
        {
            Debug.Log("�� Ÿ�Կ����� ����� �� ���� ��");

            return;
        }

        // ��
        if (currentIceCream.Type == IceCreamType.Cone)
        {
            if (currentIceCream.Flavors.Count >= 3)
            {
                return;
            }

            currentIceCream.Flavors.Add(flavor);

            Debug.Log(flavor + "�� �߰�");
        }

        // ��
        else if (currentIceCream.Type == IceCreamType.Bar)
        {
            currentIceCream.Flavors.Clear();

            currentIceCream.Flavors.Add(flavor);

            Debug.Log(flavor + "�� �߰�");
        }
    }

    [VisibleEnum(typeof(Topping))]
    public void AddTopping(int toppingIndex)
    {
        if (currentIceCream == null)
        {
            return;
        }

        Topping topping = (Topping)toppingIndex;

        currentIceCream.Toppings.Add(topping);

        Debug.Log(topping + " ���� �߰�");
    }

    [VisibleEnum(typeof(Syrup))]
    public void AddSyrup(int syrupIndex)
    {
        if (currentIceCream == null)
        {
            return;
        }

        // �ܸ� ����
        if (currentIceCream.Type != IceCreamType.Cone)
        {
            return;
        }

        Cone cone = (Cone)currentIceCream;

        if(cone.syrup != Syrup.None)
        {
            Debug.Log("�̹� �÷��� �����մϴ�");
;           return;
        }

        cone.syrup = (Syrup)syrupIndex;

        Debug.Log(cone.syrup + " �÷� �߰�");
    }

    // �ʱ�ȭ
    public void ResetIceCream()
    {
        currentIceCream = null;

        Debug.Log("���̽�ũ�� �ʱ�ȭ");
    }

    public void CompleteIceCream()
    {
        if (currentIceCream == null)
        {
            Debug.Log("���� ���� ���̽�ũ�� ����");

            return;
        }

        string result = "";

        // Ÿ��
        result += $"Ÿ�� : {currentIceCream.Type}\n";

        // ��
        result += "�� : ";

        if (currentIceCream.Flavors.Count > 0)
        {
            result += string.Join(", ", currentIceCream.Flavors);
        }
        else
        {
            result += "����";
        }

        result += "\n";

        // ����
        result += "���� : ";

        if (currentIceCream.Toppings.Count > 0)
        {
            result += string.Join(", ", currentIceCream.Toppings);
        }
        else
        {
            result += "����";
        }

        result += "\n";

        // �÷� (�ܸ�)
        if (currentIceCream.Type == IceCreamType.Cone)
        {
            Cone cone = (Cone)currentIceCream;

            result += $"�÷� : {cone.syrup}";
        }

        Debug.Log(result);
        money += 50;
        moneyText.text = money.ToString();

        ResetIceCream(); // ���� �� �ʱ�ȭ
            result += $"�÷� : {cone.syrup}";
        }

        Debug.Log(result);

        gameManager.LeaveWalk(3f);
    }
}
