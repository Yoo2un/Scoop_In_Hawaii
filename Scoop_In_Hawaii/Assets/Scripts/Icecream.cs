using System.Collections.Generic;

public class Icecream
{
    public E_Icecream_Type type { get; set; }
    public List<E_Icecream_Taste> taste { get; set; }
    public HashSet<E_Icecream_Topping> topping { get; set; }

    public override bool Equals(object obj)
    {
        if (obj == null)
        {
            return false;
        }
        if (!(obj is Icecream))
        {
            return false;
        }
        return (this.type == ((Icecream)obj).type)
          && (this.taste == ((Icecream)obj).taste)
          && (this.topping == ((Icecream)obj).topping);
    }

    public override int GetHashCode()
    {
        return type.GetHashCode() ^ taste.GetHashCode() ^ topping.GetHashCode();
    }
}
