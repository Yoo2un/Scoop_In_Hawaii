using System.Collections.Generic;
using UnityEngine;

public class OrderDialog
{
    public static string GenerateText(IceCream iceCream)
    {
        if (iceCream.coneOnly)
        {
            return "콘만 주세요!";
        }

        if (iceCream.handIceCream)
        {
            return "손에 바로 올려주세요!";
        }

        if (iceCream.anything)
        {
            return "아무거나 맛있게 해주세요!";
        }

        string text = "";

        if (iceCream.Type == IceCreamType.Bar)
        {
            text += "바로 ";

            // 맛
            text += $"맛은 {IceCreamData.FlavorNames[iceCream.Flavors[0]]} ";

            // 토핑
            if (iceCream.Toppings.Count > 0)
            {
                text += "토핑은 ";

                List<string> toppings = new List<string>();

                foreach (Topping topping in iceCream.Toppings)
                {
                    toppings.Add(IceCreamData.ToppingNames[topping]);
                }

                text += string.Join(", ", toppings);

                text += "(으)로 주세요.";
            }
            else
            {
                text += "토핑은 없이 주세요.";
            }
        }

        // Cone
        else if (iceCream.Type == IceCreamType.Cone)
        {
            Cone cone = (Cone)iceCream;

            text += "콘으로 ";

            // 맛
            text += "1층부터 ";

            List<string> flavors = new List<string>();

            foreach (Flavor flavor in cone.Flavors)
            {
                flavors.Add(IceCreamData.FlavorNames[flavor]);
            }

            text += string.Join(", ", flavors);

            text += " 순으로 주세요. ";

            // 토핑
            if (cone.Toppings.Count > 0)
            {
                text += "토핑은 ";

                List<string> toppings = new List<string>();

                foreach (Topping topping in cone.Toppings)
                {
                    toppings.Add(IceCreamData.ToppingNames[topping]);
                }

                text += string.Join(", ", toppings);

                text += "(으)로 해주세요. ";
            }
            else
            {
                text += "토핑은 없이 해주세요. ";
            }

            // 시럽
            if (cone.syrup != Syrup.None)
            {
                text += $"시럽은 {IceCreamData.SyrupNames[cone.syrup]}로 부탁드려요.";
            }
            else
            {
                text += "시럽은 안 뿌려주셔도 돼요.";
            }
        }

        return text;
    }
}