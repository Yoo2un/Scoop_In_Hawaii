public class Icecream_Cone : Icecream
{
    public int cone { get; set; }
    public E_Syrup_Taste syrup_Taste { get; set; }

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
        return base.Equals(obj)
          && (this.cone == ((Icecream_Cone)obj).cone)
          && (this.syrup_Taste == ((Icecream_Cone)obj).syrup_Taste);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode() ^ cone.GetHashCode() ^ syrup_Taste.GetHashCode();
    }
}
