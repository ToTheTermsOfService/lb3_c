using Lb3.Interfaces;

namespace Lb3;
class Person : INameAndCopy
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public string Name { get => FirstName + " " + LastName; set { } }

    public override bool Equals(object? obj)
    {
        if (obj is Person other)
            return FirstName == other.FirstName && LastName == other.LastName && BirthDate == other.BirthDate;
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(FirstName, LastName, BirthDate);
    }

    public static bool operator ==(Person? p1, Person? p2)
    {
        return Equals(p1, p2);
    }

    public static bool operator !=(Person? p1, Person? p2)
    {
        return !Equals(p1, p2);
    }

    public virtual object DeepCopy()
    {
        return new Person { FirstName = FirstName, LastName = LastName, BirthDate = BirthDate };
    }
}