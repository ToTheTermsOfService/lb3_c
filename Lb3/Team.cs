using Lb3.Interfaces;
using System.Text.Json.Serialization;

namespace Lb3;
[Serializable]
public class Team : INameAndCopy, IComparable<Team>
{
    protected string _organization;
    protected int _registrationNumber;

    public Team() : this("Стандартна команда", 1)
    {
    }

    [JsonConstructor]
    public Team(string organization, int registrationNumber)
    {
        Organization = organization;
        RegistrationNumber = registrationNumber;
    }

    public string Organization
    {
        get => _organization;
        set => _organization = value;
    }

    public int RegistrationNumber
    {
        get => _registrationNumber;
        set
        {
            if (value <= 0)
                throw new ArgumentException("Число має бути додатнім");
            _registrationNumber = value;
        }
    }

    public string Name
    {
        get => Organization;
        set => Organization = value;
    }

    public virtual object DeepCopy()
    {
        return new Team(Organization, RegistrationNumber);
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        var other = (Team)obj;
        return Organization == other.Organization &&
               RegistrationNumber == other.RegistrationNumber;
    }

    public static bool operator ==(Team? left, Team? right)
    {
        if (ReferenceEquals(left, right)) return true;
        if (left is null || right is null) return false;
        return left.Equals(right);
    }

    public static bool operator !=(Team? left, Team? right)
    {
        return !(left == right);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Organization, RegistrationNumber);
    }

    public override string ToString()
    {
        return $"Організація: {Organization}, Реєстрація: {RegistrationNumber}";
    }

    public int CompareTo(Team? other)
    {
        return other == null ? 1 : RegistrationNumber.CompareTo(other.RegistrationNumber);
    }
}
