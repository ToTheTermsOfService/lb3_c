using System.Diagnostics;

namespace Lb3.Helpers;

class TestCollections
{
    private List<Team> _teams;
    private List<string> _strings;
    private Dictionary<Team, ResearchTeam> _teamDict;
    private Dictionary<string, ResearchTeam> _stringDict;

    public TestCollections(int count)
    {
        _teams = new List<Team>();
        _strings = new List<string>();
        _teamDict = new Dictionary<Team, ResearchTeam>();
        _stringDict = new Dictionary<string, ResearchTeam>();

        for (int i = 0; i < count; i++)
        {
            ResearchTeam rt = GenerateResearchTeam(i);
            Team teamKey = new Team(rt.Organization, rt.RegNumber);
            string stringKey = teamKey.ToString();

            _teams.Add(teamKey);
            _strings.Add(stringKey);
            _teamDict.Add(teamKey, rt);
            _stringDict.Add(stringKey, rt);
        }
    }

    public static ResearchTeam GenerateResearchTeam(int value)
    {
        return new ResearchTeam($"Org{value}", value, $"Topic{value}", (TimeFrame)(value % 3));
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
            ResearchTeam value = i < 3 ? _teamDict[teamKey] : GenerateResearchTeam(int.MaxValue);

            Console.WriteLine($"\nTesting {positions[i]} element:");

            MeasureTime(() => _teams.Contains(teamKey), "List<Team>.Contains");
            MeasureTime(() => _strings.Contains(stringKey), "List<string>.Contains");
            MeasureTime(() => _teamDict.ContainsKey(teamKey), "Dictionary<Team, ResearchTeam>.ContainsKey");
            MeasureTime(() => _stringDict.ContainsKey(stringKey), "Dictionary<string, ResearchTeam>.ContainsKey");
            MeasureTime(() => _teamDict.ContainsValue(value), "Dictionary<Team, ResearchTeam>.ContainsValue");
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