namespace webenology.blazor.components.dropdown;

public class DropDownItem<T> : IEqualityComparer<DropDownItem<T>?>
{
    public T Key { get; set; }
    public string Value { get; set; }
    internal bool IsSelected { get; set; }
    public bool IsDisabled { get; set; }

    public DropDownItem() : this(default!, string.Empty)
    {

    }
    public DropDownItem(T key, string value)
    {
        if (!(typeof(T) == typeof(string) || typeof(T) == typeof(int)))
        {
            throw new Exception("You can only have it be a type of int or type of string");
        }
        Key = key;
        Value = value;
    }

    public bool Equals(DropDownItem<T>? x, DropDownItem<T>? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        if (x.GetType() != y.GetType()) return false;
        return EqualityComparer<T>.Default.Equals(x.Key, y.Key);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((DropDownItem<T>)obj);
    }

    protected bool Equals(DropDownItem<T> other)
    {
        return EqualityComparer<T>.Default.Equals(Key, other.Key);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Key, Value, IsSelected, IsDisabled);
    }

    public int GetHashCode(DropDownItem<T>? obj)
    {
        return base.GetHashCode();
    }
}