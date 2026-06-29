using TMPro;
using UnityEngine;
using System.Collections;

public class IceCreamMaking : MonoBehaviour, IDataPersistence
{
    public IceCream currentIceCream;
    public TMP_Text moneyText;
    private int money = 0;

    private GameManager gameManager;
    private bool BarProcessing = false;

    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        GameManager.Instance.setMoney(money);
    }

    public void LoadData(GameData data)
    {
        this.money = data.money;
        moneyText.text = money.ToString();
    }

    public void SaveData(ref GameData data)
    {
        data.money = this.money;
    }

    [VisibleEnum(typeof(IceCreamType))]
    public void SelectType(int type)
    {
        if (type == 0) //콘이면
        {
            currentIceCream = new Cone();
        }
        else //바이면
        {
            currentIceCream = new IceCream();
        }
        currentIceCream.Type = (IceCreamType)type;

        Debug.Log(currentIceCream.Type + "추가");
    }


    [VisibleEnum(typeof(Flavor))]
    public void AddFlavor(int flavorIndex)
    {
        if (currentIceCream == null)
        {
            return;
        }

        // ������ �̹� �ö��ִ��� �˻�
        if (currentIceCream.Toppings.Count > 0)
        {
            Debug.Log("");
            return;
        }

        // �÷��� �̹� �ö��ִ��� �˻�
        if (currentIceCream.Type == IceCreamType.Cone)
        {
            Cone cone = (Cone)currentIceCream;

            if (cone.syrup != Syrup.None)
            {
                Debug.Log("이미 시럽이 존재합니다");
                return;
            }
        }

        Flavor flavor = (Flavor)flavorIndex;

        // 현재 타입에서 사용 가능한 맛인지 검사
        if (!IceCreamData.TypeFlavors[currentIceCream.Type].Contains(flavor))
        {
            Debug.Log("이 타입에서는 사용할 수 없는 맛");

            return;
        }

        // 콘
        if (currentIceCream.Type == IceCreamType.Cone)
        {
            if (currentIceCream.Flavors.Count >= 3)
            {
                return;
            }

            currentIceCream.Flavors.Add(flavor);

            Debug.Log(flavor + "맛 추가");

            GameManager.Instance.material_cost += 10;
            Debug.Log("(임시)재료비 10원 증가");
        }

        // 바
        else if (currentIceCream.Type == IceCreamType.Bar)
        {
            if (BarProcessing) return;
            currentIceCream.Flavors.Clear();

            currentIceCream.Flavors.Add(flavor);

            Debug.Log(flavor + "맛 추가");

            GameManager.Instance.material_cost += 10;
            Debug.Log("(임시)재료비 10원 증가");
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

        Debug.Log(topping + " 토핑 추가");
    }

    [VisibleEnum(typeof(Syrup))]
    public void AddSyrup(int syrupIndex)
    {
        if (currentIceCream == null)
        {
            return;
        }

        // 콘만 가능
        if (currentIceCream.Type != IceCreamType.Cone)
        {
            return;
        }

        Cone cone = (Cone)currentIceCream;

        if (cone.syrup != Syrup.None)
        {
            Debug.Log("시럽이 이미 있습니다.");
            ; return;
        }

        cone.syrup = (Syrup)syrupIndex;

        Debug.Log(cone.syrup + " 시럽 추가");
    }

    // 바 제작 시작
    public void ProcessingIceCream()
    {
        if (BarProcessing== true)
        {
            Debug.Log("제작 중");
            return;
        }

        StartCoroutine(ProduceCoroutine());
    }

    // 바 제작 쿨타임
    private IEnumerator ProduceCoroutine()
    {
        BarProcessing = true;
        Debug.Log("아이스크림 제작 시작");

        yield return new WaitForSeconds(3.0f);

        BarProcessing = false;
        Debug.Log("제작 완료");
    }

    // 초기화
    public void ResetIceCream()
    {
        currentIceCream = null;

        Debug.Log("아이스크림 초기화");
    }

    public void CompleteIceCream()
    {
        if (currentIceCream == null)
        {
            Debug.Log("제작 중인 아이스크림 없음");

            return;
        }

        string result = "";

        // 타입
        result += $"타입 : {currentIceCream.Type}\n";

        // 맛
        result += "맛 : ";

        if (currentIceCream.Flavors.Count > 0)
        {
            result += string.Join(", ", currentIceCream.Flavors);
        }
        else
        {
            result += "없음";
        }

        result += "\n";

        // 토핑
        result += "토핑 : ";

        if (currentIceCream.Toppings.Count > 0)
        {
            result += string.Join(", ", currentIceCream.Toppings);
        }
        else
        {
            result += "없음";
        }

        result += "\n";

        // 시럽 (콘만)
        if (currentIceCream.Type == IceCreamType.Cone)
        {
            Cone cone = (Cone)currentIceCream;

            result += $"시럽 : {cone.syrup}";
        }

        Debug.Log(result);

        money += 50;
        if (GameManager.Instance != null)
        {
            GameManager.Instance.profit += 50;
        }
        GameManager.Instance.setMoney(money);
        moneyText.text = money.ToString();

        ResetIceCream(); // 완료 후 초기화

        gameManager.LeaveWalk(3f);
    }
}