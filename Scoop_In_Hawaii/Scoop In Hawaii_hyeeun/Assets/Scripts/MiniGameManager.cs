using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IceCream;

public class MiniGameManager : MonoBehaviour
{
    public int Day;
    public int Time;
    public List<IceCream> Orders = new List<IceCream>();
    public int money;
    public IceCream currentIceCream;

    public void RandomOrder()
    {
        Orders.Clear();

        IceCream newOrder;
        if (Random.Range(0, 2) == 0)
        {
            newOrder = new Cone() { Type = IceCreamType.Cone };
            int scoops = Random.Range(1, 3);
            for (int i = 0; i < scoops; i++)
            {
                newOrder.TasteList.Add((Taste)Random.Range(0, 2));
            }

            if (Random.Range(0, 2) == 0)
            {
                newOrder.SyrupSet.Add((Topping)Random.Range(2, 4));
            }
        }
        else
        {
            newOrder = new IceCream() { Type = IceCreamType.Bar };
            newOrder.TasteList.Add((Taste)Random.Range(2, 4));
        }
        int toppings = Random.Range(0, 3);
        for (int i = 0; i < toppings; i++)
        {
            newOrder.ToppingSet.Add((Topping)Random.Range(0, 2));
        }

        Orders.Add(newOrder);
        Debug.Log("주문: " + newOrder.Type + " 맛: " + string.Join(", ", newOrder.TasteList) + " 시럽: " + string.Join(", ", newOrder.SyrupSet) + " 토핑: " + string.Join(", ", newOrder.ToppingSet));
    }

    public void MakeIceCream()
    {
        Debug.Log("제작: " + currentIceCream.Type + " 맛: " + string.Join(", ", currentIceCream.TasteList) + " 시럽: " + string.Join(", ", currentIceCream.SyrupSet) + " 토핑: " + string.Join(", ", currentIceCream.ToppingSet));
    }

    public void StartCone()
    {
        currentIceCream = new Cone();
        currentIceCream.Type = IceCreamType.Cone;
        Debug.Log("콘 제작 시작");
    }

    public void StartBar()
    {
        currentIceCream = new IceCream();
        currentIceCream.Type = IceCreamType.Bar;
        Debug.Log("바 제작 시작");
    }

    public void AddScoop(int tasteIndex)
    {
        if (currentIceCream is Cone cone)
        {
            if (cone.ToppingSet.Count > 0 || cone.SyrupSet.Count > 0) return;
            if (cone.ScoopCount >= 2) return;

            Taste taste = (Taste)tasteIndex;
            cone.TasteList.Add(taste);
            cone.ScoopCount++;
            Debug.Log("스쿱 추가: " + taste);
        }
    }

    public void SetBarBase(int tasteIndex)
    {
        if (currentIceCream.Type == IceCreamType.Bar)
        {
            currentIceCream.TasteList.Clear();
            Taste taste = (Taste)tasteIndex;
            currentIceCream.TasteList.Add(taste);
            Debug.Log("바 베이스: " + taste);
        }
    }

    public void AddSyrup(int toppingIndex)
    {
        if (currentIceCream is Cone cone)
        {
            Topping topping = (Topping)toppingIndex;
            cone.SyrupSet.Add(topping);
            Debug.Log("시럽 추가: " + topping);
        }
    }

    public void AddTopping(int toppingIndex)
    {
        Topping topping = (Topping)toppingIndex;
        currentIceCream.ToppingSet.Add(topping);
        Debug.Log("토핑 추가: " + topping);
    }
}