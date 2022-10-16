using FreeCourse.Services.Order.Domain.Core;

namespace FreeCourse.Services.Order.Domain.OrderAggregate;

public class Address : ValueObject
{
    public Address(string province, string district, string street, string zipCode, string line)
    {
        Province = province;
        District = district;
        Street = street;
        ZipCode = zipCode;
        Line = line;
    }

    public string Province { get; }

    public string District { get; }

    public string Street { get; }

    public string ZipCode { get; }

    public string Line { get; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Province;
        yield return District;
        yield return Street;
        yield return ZipCode;
        yield return Line;
    }
}