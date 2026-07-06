using System.Collections.Generic;
using UnityEngine;

public class OrderGenerator : MonoBehaviour
{
    public static IceCream GenerateOrder()
    {

        Debug.Log($"GenerateOrder 호출 / CurrentTrendFlavor = {TrendFlavorModifier.Instance.CurrentTrendFlavor}");

        // Cone(1) Bar(0)
        int type;

        if (TrendFlavorModifier.Instance != null && TrendFlavorModifier.Instance.CurrentTrendFlavor != Flavor.None)
        {
            // 80%는 유행하는 종류
            if (Random.value < 0.8f)
            {
                type = TrendFlavorModifier.Instance.CurrentTrendType == IceCreamType.Cone ? 1 : 0;
            }
            else
            {
                type = Random.Range(0, 2);
            }
        }
        else
        {
            type = Random.Range(0, 2);
        }

        // Cone 생성
        if (type == 1)
        {
            Cone cone = new Cone();

            cone.Type = IceCreamType.Cone;

            // 스쿱 개수
            int scoopCount = Random.Range(1, 4);

            for (int i = 0; i < scoopCount; i++)
            {
                List<Flavor> flavors = IceCreamData.TypeFlavors[IceCreamType.Cone];

                Flavor randomFlavor = flavors[Random.Range(0, flavors.Count)];

                cone.Flavors.Add(randomFlavor);
            }

            if (TrendFlavorModifier.Instance != null && TrendFlavorModifier.Instance.CurrentTrendType == IceCreamType.Cone && TrendFlavorModifier.Instance.CurrentTrendFlavor != Flavor.None)
            {
                Flavor trend = TrendFlavorModifier.Instance.CurrentTrendFlavor;

                //80%는 유행하는 맛으로
                if (Random.value < 0.8f)
                {
                    if (!cone.Flavors.Contains(trend))
                    {
                        int index = Random.Range(0, cone.Flavors.Count);

                        cone.Flavors[index] = trend;
                    }
                }
            }

            // 토핑 개수
            int toppingCount = Random.Range(0, 3);

            for (int i = 0; i < toppingCount; i++)
            {
                Topping randomTopping =
                    (Topping)Random.Range(
                        1,
                        System.Enum.GetValues(typeof(Topping)).Length
                    );

                cone.Toppings.Add(randomTopping);
            }

            // 시럽 50%
            if (Random.Range(0, 2) == 1)
            {
                cone.syrup =
                    (Syrup)Random.Range(
                        1,
                        System.Enum.GetValues(typeof(Syrup)).Length
                    );
            }

            int specialChance;

            if (TrendFlavorModifier.Instance != null && TrendFlavorModifier.Instance.CurrentTrendFlavor != Flavor.None)
            {
                specialChance = 100;   // 특수 주문 안 나옴
            }
            else
            {
                specialChance = Random.Range(0, 100);
            }


            // 콘만 주세요
            if (specialChance < 10)
            {
                cone.coneOnly = true;

                cone.Flavors.Clear();
                cone.Toppings.Clear();
                cone.syrup = Syrup.None;
            }

            // 손에 올려주세요
            else if (specialChance < 20)
            {
                cone.handIceCream = true;
            }

            // 아무거나 주세요
            else if (specialChance < 30)
            {
                cone.anything = true;
            }

            return cone;
        }

        // Bar 생성
        else
        {
            IceCream bar = new IceCream();

            bar.Type = IceCreamType.Bar;

            // 바는 맛 1개
            List<Flavor> flavors = IceCreamData.TypeFlavors[IceCreamType.Bar];

            Flavor randomFlavor = flavors[Random.Range(0, flavors.Count)];

            if (TrendFlavorModifier.Instance != null && TrendFlavorModifier.Instance.CurrentTrendType == IceCreamType.Bar && TrendFlavorModifier.Instance.CurrentTrendFlavor != Flavor.None)
            {
                bar.Flavors.Clear();
                bar.Flavors.Add(TrendFlavorModifier.Instance.CurrentTrendFlavor);
            }
            else
            {
                bar.Flavors.Add(randomFlavor);
            }

            // 토핑 개수
            int toppingCount = Random.Range(0, 3);

            for (int i = 0; i < toppingCount; i++)
            {
                Topping randomTopping =
                    (Topping)Random.Range(
                        1,
                        System.Enum.GetValues(typeof(Topping)).Length
                    );

                bar.Toppings.Add(randomTopping);
            }

            int specialChance;

            if (TrendFlavorModifier.Instance != null && TrendFlavorModifier.Instance.CurrentTrendFlavor != Flavor.None)
            {
                specialChance = 100;   // 특수 주문 안 나옴
            }
            else
            {
                specialChance = Random.Range(0, 100);
            }

            // 아무거나 주세요
            if (specialChance < 15)
            {
                bar.anything = true;
            }

            return bar;
        }
    }
}