using System.Collections.Generic;
using UnityEngine;

public static class IceCreamData
{
    public static Dictionary<IceCreamType, List<Flavor>> TypeFlavors =
    new Dictionary<IceCreamType, List<Flavor>>()
    {
        { IceCreamType.Cone, new List<Flavor> { Flavor.Vanilla, Flavor.Chocolate } },
        { IceCreamType.Bar, new List<Flavor> { Flavor.Soda, Flavor.Mango } }
    };

    public static Dictionary<Flavor, string> FlavorNames =
    new Dictionary<Flavor, string>()
    {
        { Flavor.Vanilla, "바닐라" },
        { Flavor.Chocolate, "초코" },
        { Flavor.Soda, "소다" },
        { Flavor.Mango, "망고" }
    };

    public static Dictionary<Topping, string> ToppingNames =
    new Dictionary<Topping, string>()
    {
        { Topping.PineApple, "파인애플" },
        { Topping.Sprinkles, "스프링클" }
    };

    public static Dictionary<Syrup, string> SyrupNames =
    new Dictionary<Syrup, string>()
    {
        { Syrup.Chocolate, "초코" },
        { Syrup.Caramel, "카라멜" }
    };

    public static Dictionary<Flavor, List<int>> FlavorPrices =
    new Dictionary<Flavor, List<int>>
    {
        { Flavor.Vanilla, new List<int> {180, 1300} },
        { Flavor.Chocolate, new List<int> {220, 1600} },
        { Flavor.Soda, new List<int> {100, 800} },
        { Flavor.Mango, new List<int> {120, 950} }
    };

    public static Dictionary<Flavor, int> FlavorStocks =
    new Dictionary<Flavor, int>()
    {
        { Flavor.Vanilla, 20 },
        { Flavor.Chocolate, 20 },
        { Flavor.Soda, 10 },
        { Flavor.Mango, 10 }
    };

    public static Dictionary<Flavor, int> MaxFlavorStocks =
    new Dictionary<Flavor, int>()
    {
        { Flavor.Vanilla, 20 },
        { Flavor.Chocolate, 20 },
        { Flavor.Soda, 10 },
        { Flavor.Mango, 10 }
    };
}
