using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class IceCream
{
    public bool coneOnly;
    public bool handIceCream;
    public bool anything;
    public IceCreamType Type { get; set; }

    public List<Flavor> Flavors { get; set; } = new List<Flavor>();

    public HashSet<Topping> Toppings { get; set; } = new HashSet<Topping>();

    public override bool Equals(object obj)
    {
        if (obj == null || !(obj is IceCream)) return false;

        return (this.Type == ((IceCream)obj).Type)
           && (this.Flavors.SequenceEqual(((IceCream)obj).Flavors))
           && (this.Toppings.SetEquals(((IceCream)obj).Toppings));
    }

    public override int GetHashCode()
    {
        int hash = Type.GetHashCode();

        foreach (Flavor flavor in Flavors)
        {
            hash ^= flavor.GetHashCode();
        }

        foreach (Topping topping in Toppings)
        {
            hash ^= topping.GetHashCode();
        }

        return hash;
    }
}
