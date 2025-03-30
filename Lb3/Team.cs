using Lb3.Interfaces;

namespace Lb3;
class Team : INameAndCopy, IComparable<Team>
{
    protected string _organization = string.Empty;
    protected int _regNumber;

    public string Organization
    {
        get => _organization;
        set => _organization = value;
    }

    public int RegNumber
    {
        get => _regNumber;
        set
        {
            if (value < 0)
                throw new ArgumentException("Registration number must be positive.");
            _regNumber = value;
        }
    }

    public string Name { get => Organization; set { } }

    public Team(string org, int num)
    {
        Organization = org;
        RegNumber = num;
    }

    public Team() : this("Default Organization", 1) { }

    public virtual object DeepCopy()
    {
        return new Team(Organization, RegNumber);
    }

    public override bool Equals(object? obj)
    {
        if (obj is Team other)
            return Organization == other.Organization && RegNumber == other.RegNumber;
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Organization, RegNumber);
    }

    public static bool operator ==(Team? t1, Team? t2)
    {
        return Equals(t1, t2);
    }

    public static bool operator !=(Team? t1, Team? t2)
    {
        return !Equals(t1, t2);
    }

    public override string ToString()
    {
        return $"Team: {Organization}, RegNumber: {RegNumber}";
    }

    public int CompareTo(Team? other)
    {
        if (other == null) return 1;
        return RegNumber.CompareTo(other.RegNumber);
    }
}
