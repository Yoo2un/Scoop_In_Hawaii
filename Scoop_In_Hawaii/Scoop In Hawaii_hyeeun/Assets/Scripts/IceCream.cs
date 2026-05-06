using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class IceCream
{
    public IceCreamType Type;
    public List<Taste> TasteList = new List<Taste>();
    public HashSet<Topping> ToppingSet = new HashSet<Topping>();
    public HashSet<Topping> SyrupSet = new HashSet<Topping>();



    public enum IceCreamType
    {
        Cone, 
        Bar
    }

    public enum Taste
    {
        Vanila,
        Chocolate,
        Mango,
        Soda
    }

    public enum Topping
    {
        Pineapple,
        Sprinkles,
        ChocolateSyrup,
        Caramelsyrup
    }
}
