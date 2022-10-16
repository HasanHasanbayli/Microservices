namespace FreeCourse.Services.Order.Domain.Core;

public abstract class Entity
{
    private int? _requestedHashCode;

    public virtual int Id { get; set; }

    public bool IsTransient()
    {
        return Id == default;
    }

    public override int GetHashCode()
    {
        if (!IsTransient())
        {
            if (!_requestedHashCode.HasValue)
                _requestedHashCode =
                    Id.GetHashCode() ^
                    31; // XOR for random distribution (http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx)

            return _requestedHashCode.Value;
        }

        return base.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        if (obj == null || !(obj is Entity))
            return false;

        if (ReferenceEquals(this, obj))
            return true;

        if (GetType() != obj.GetType())
            return false;

        var item = (Entity)obj;

        if (item.IsTransient() || IsTransient())
            return false;
        return item.Id == Id;
    }

    public static bool operator ==(Entity left, Entity right)
    {
        if (Equals(left, null))
            return Equals(right, null) ? true : false;
        return left.Equals(right);
    }

    public static bool operator !=(Entity left, Entity right)
    {
        if (Equals(left, null))
            return Equals(right, null) ? true : false;
        return left.Equals(right);
    }
}