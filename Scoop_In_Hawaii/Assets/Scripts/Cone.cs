using UnityEngine;

public class Cone : IceCream
{
    public Syrup syrup = Syrup.None;

    public override bool Equals(object obj)
    {
        if (obj == null || !(obj is Cone)) return false;

        return base.Equals(obj)
          && (this.syrup == ((Cone)obj).syrup);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode() ^ syrup.GetHashCode();
    }
}
