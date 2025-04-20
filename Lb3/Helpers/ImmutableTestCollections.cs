using System.Collections.Immutable;
using System.Diagnostics;

namespace Lb3.Helpers;
class ImmutableTestCollections
{
    private ImmutableList<Team> _teams;
    private ImmutableList<string> _strings;
    private ImmutableDictionary<Team, ResearchTeam> _teamDict;
    private ImmutableDictionary<string, ResearchTeam> _stringDict;

    public ImmutableTestCollections(int count)
    {
        var teamsBuilder = ImmutableList.CreateBuilder<Team>();
        var stringsBuilder = ImmutableList.CreateBuilder<string>();
        var teamDictBuilder = ImmutableDictionary.CreateBuilder<Team, ResearchTeam>();
        var stringDictBuilder = ImmutableDictionary.CreateBuilder<string, ResearchTeam>();

        for (int i = 0; i < count; i++)
        {
            ResearchTeam rt = TestCollections.GenerateResearchTeam(i);
            Team teamKey = new Team(rt.Organization, rt.RegNumber);
            string stringKey = teamKey.ToString();

            teamsBuilder.Add(teamKey);
            stringsBuilder.Add(stringKey);
            teamDictBuilder.Add(teamKey, rt);
            stringDictBuilder.Add(stringKey, rt);
        }

        _teams = teamsBuilder.ToImmutable();
        _strings = stringsBuilder.ToImmutable();
        _teamDict = teamDictBuilder.ToImmutable();
        _stringDict = stringDictBuilder.ToImmutable();
    }

    public void MeasureSearchTime()
    {
        if (_teams.Count == 0) return;

        int[] testIndices = { 0, _teams.Count / 2, _teams.Count - 1, _teams.Count };
        string[] positions = { "first", "middle", "last", "nonexistent" };

        for (int i = 0; i < testIndices.Length; i++)
        {
            Team teamKey = i < 3 ? _teams[testIndices[i]] : new Team("Nonexistent", int.MaxValue);
            string stringKey = i < 3 ? _strings[testIndices[i]] : "Nonexistent";
            ResearchTeam value = i < 3 ? _teamDict[teamKey] : TestCollections.GenerateResearchTeam(int.MaxValue);

            Console.WriteLine($"\nTesting {positions[i]} element (Immutable):");

            MeasureTime(() => _teams.Contains(teamKey), "ImmutableList<Team>.Contains");
            MeasureTime(() => _strings.Contains(stringKey), "ImmutableList<string>.Contains");
            MeasureTime(() => _teamDict.ContainsKey(teamKey), "ImmutableDictionary<Team, ResearchTeam>.ContainsKey");
            MeasureTime(() => _stringDict.ContainsKey(stringKey), "ImmutableDictionary<string, ResearchTeam>.ContainsKey");
            MeasureTime(() => _teamDict.ContainsValue(value), "ImmutableDictionary<Team, ResearchTeam>.ContainsValue");
        }
    }

    private void MeasureTime(Action action, string description)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        action();
        sw.Stop();
        Console.WriteLine($"{description}: {sw.ElapsedTicks} ticks");
    }
}
