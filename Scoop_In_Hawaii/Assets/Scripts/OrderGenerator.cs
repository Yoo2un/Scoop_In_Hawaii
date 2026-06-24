using System.Collections.Generic;
using UnityEngine;

public class OrderGenerator : MonoBehaviour
{
    public static IceCream GenerateOrder()
    {
        // Cone(1) Bar(0)
        int type = Random.Range(0, 2);

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

            int specialChance = Random.Range(0, 100);

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

            bar.Flavors.Add(randomFlavor);

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

            int specialChance = Random.Range(0, 100);

            // 아무거나 주세요
            if (specialChance < 15)
            {
                bar.anything = true;
            }

            return bar;
        }
    }
}