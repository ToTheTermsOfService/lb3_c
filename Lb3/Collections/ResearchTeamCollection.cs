using Lb3.Comparers;
using Lb3.Helpers;

namespace Lb3.Collections;
class ResearchTeamCollection
{
    private List<ResearchTeam> _researchTeams = new List<ResearchTeam>();

    public void AddDefaults()
    {
        _researchTeams.Add(new ResearchTeam("Org1", 1, "Topic1", TimeFrame.Year));
        _researchTeams.Add(new ResearchTeam("Org2", 2, "Topic2", TimeFrame.TwoYears));
        _researchTeams.Add(new ResearchTeam("Org3", 3, "Topic3", TimeFrame.Long));
    }

    public void AddResearchTeams(params ResearchTeam[] teams)
    {
        if (teams == null)
        {
            throw new ArgumentNullException(nameof(teams), "Teams array cannot be null");
        }
        _researchTeams.AddRange(teams);
    }

    public override string ToString()
    {
        return string.Join("\n\n", _researchTeams.Select(rt => rt.ToString()));
    }

    public string ToShortList()
    {
        return string.Join("\n", _researchTeams.Select(rt => rt.ToShortString()));
    }

    public void SortByRegNumber()
    {
        _researchTeams.Sort();
    }

    public void SortByResearchTopic()
    {
        _researchTeams.Sort(new ResearchTeam());
    }

    public void SortByPublicationsCount()
    {
        _researchTeams.Sort(new ResearchTeamPublicationsComparer());
    }

    public int MinRegNumber => _researchTeams.Count == 0 ? 0 : _researchTeams.Min(rt => rt.RegNumber);

    public IEnumerable<ResearchTeam> TwoYearsDurationTeams =>
        _researchTeams.Where(rt => rt.Duration == TimeFrame.TwoYears);

    public List<ResearchTeam> NGroup(int value) =>
        _researchTeams.Where(rt => rt.Participants.Count == value).ToList();
}