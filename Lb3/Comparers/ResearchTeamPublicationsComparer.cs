namespace Lb3.Comparers;
class ResearchTeamPublicationsComparer : IComparer<ResearchTeam>
{
    public int Compare(ResearchTeam? x, ResearchTeam? y)
    {
        if (x == null && y == null) return 0;
        if (x == null) return -1;
        if (y == null) return 1;
        return x.Publications.Count.CompareTo(y.Publications.Count);
    }
}