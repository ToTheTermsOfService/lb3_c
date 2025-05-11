using Lb3.Interfaces;

namespace Lb3;
[Serializable]
public class Person
{
    private string _firstName;
    private string _lastName;
    private DateTime _birthDate;

    public Person()
    {
        _firstName = "Дід";
        _lastName = "Мороз";
        _birthDate = new DateTime(2005, 5, 5);
    }

    public Person(string firstName, string lastName, DateTime birthDate)
    {
        FirstName = firstName;
        LastName = lastName;
        BirthDate = birthDate;
    }

    public string FirstName
    {
        get => _firstName;
        set => _firstName = value;
    }

    public string LastName
    {
        get => _lastName;
        set => _lastName = value;
    }

    public DateTime BirthDate
    {
        get => _birthDate;
        set => _birthDate = value;
    }

    public int BirthYear
    {
        get => BirthDate.Year;
        set => BirthDate = BirthDate.AddYears(value - BirthDate.Year);
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        Person other = (Person)obj;
        return FirstName == other.FirstName &&
               LastName == other.LastName &&
               BirthDate == other.BirthDate;
    }

    public static bool operator ==(Person? left, Person? right)
    {
        if (ReferenceEquals(left, right)) return true;
        if (left is null || right is null) return false;
        return left.Equals(right);
    }

    public static bool operator !=(Person? left, Person? right)
    {
        return !(left == right);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(FirstName, LastName, BirthDate);
    }

    public virtual object DeepCopy() => new Person(FirstName, LastName, BirthDate);

    public override string ToString()
    {
        return $"{FirstName} {LastName}, народжений(-а) {BirthDate:d}";
    }

    public virtual string ToShortString()
    {
        return $"{FirstName} {LastName}";
    }
}