using UnityEngine;

public class Bar : IceCream
{
    public enum Frozen {
        None,
        HALF,
        PERFECT,
        DEEP
    };
    public Frozen status = Frozen.None;

    public override bool Equals(object obj)
    {
        if (obj == null || !(obj is Bar)) return false;

        return base.Equals(obj)
          && (this.status == ((Bar)obj).status);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode() ^ status.GetHashCode();
    }
}
