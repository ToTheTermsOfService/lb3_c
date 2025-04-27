using Lb3.Comparers;
using Lb3.EventHandlers;
using Lb3.Helpers;

namespace Lb3.Collections;
public delegate void TeamListHandler(object source, TeamListHandlerEventArgs args);

class ResearchTeamCollection
{
    private List<ResearchTeam> _researchTeams = new List<ResearchTeam>();

    public string CollectionName { get; set; }

    public event TeamListHandler ResearchTeamAdded;
    public event TeamListHandler ResearchTeamInserted;

    public void AddDefaults()
    {
        _researchTeams.Add(new ResearchTeam("Org1", 1, "Topic1", TimeFrame.Year));
        OnResearchTeamAdded(_researchTeams.Count - 1);
        _researchTeams.Add(new ResearchTeam("Org2", 2, "Topic2", TimeFrame.TwoYears));
        OnResearchTeamAdded(_researchTeams.Count - 1);
        _researchTeams.Add(new ResearchTeam("Org3", 3, "Topic3", TimeFrame.Long));
        OnResearchTeamAdded(_researchTeams.Count - 1);
    }

    public void AddResearchTeams(params ResearchTeam[] teams)
    {
        if (teams == null)
        {
            throw new ArgumentNullException(nameof(teams), "Teams array cannot be null");
        }

        foreach (var team in teams)
        {
            _researchTeams.Add(team);
            OnResearchTeamAdded(_researchTeams.Count - 1);
        }
    }

    public void InsertAt(int j, ResearchTeam researchTeam)
    {
        if (j >= 0 && j < _researchTeams.Count)
        {
            _researchTeams.Insert(j, researchTeam);
            OnResearchTeamInserted(j);
        }
        else
        {
            _researchTeams.Add(researchTeam);
            OnResearchTeamAdded(_researchTeams.Count - 1);
        }
    }

    public ResearchTeam this[int index]
    {
        get => _researchTeams[index];
        set
        {
            _researchTeams[index] = value;
            OnResearchTeamInserted(index);
        }
    }

    protected virtual void OnResearchTeamAdded(int index)
    {
        ResearchTeamAdded?.Invoke(this,
            new TeamListHandlerEventArgs(CollectionName, "Element added to the end", index));
    }

    protected virtual void OnResearchTeamInserted(int index)
    {
        ResearchTeamInserted?.Invoke(this,
            new TeamListHandlerEventArgs(CollectionName, "Element inserted at position", index));
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
