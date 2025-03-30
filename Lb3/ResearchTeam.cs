using Lb3;
using Lb3.Helpers;
using System.Collections;

class ResearchTeam : Team, IEnumerable<Person>, IComparer<ResearchTeam>
{
    private string _researchTopic;
    private TimeFrame _duration;
    private List<Person> _participants;
    private List<Paper> _publications;

    public ResearchTeam(string org, int num, string topic, TimeFrame timeFrame)
        : base(org, num)
    {
        _researchTopic = topic;
        _duration = timeFrame;
        _participants = new List<Person>();
        _publications = new List<Paper>();
    }

    public ResearchTeam() : this("Default Org", 1, "Default Topic", TimeFrame.Year) { }

    public string ResearchTopic => _researchTopic;
    public TimeFrame Duration => _duration;
    public List<Paper> Publications => _publications;
    public List<Person> Participants => _participants;
    public Team BaseTeam => new Team(Organization, RegNumber);
    public Paper? LatestPublication => _publications.Count > 0 ? _publications[^1] : null;

    public void AddPapers(params Paper[] papers)
    {
        _publications.AddRange(papers);
    }

    public void AddParticipants(params Person[] persons)
    {
        _participants.AddRange(persons);
    }

    public override object DeepCopy()
    {
        ResearchTeam copy = new ResearchTeam(Organization, RegNumber, _researchTopic, _duration)
        {
            _participants = _participants.Select(p => (Person)p.DeepCopy()).ToList(),
            _publications = _publications.Select(p => (Paper)p.DeepCopy()).ToList()
        };
        return copy;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not ResearchTeam other) return false;
        return base.Equals(other) &&
               _researchTopic == other._researchTopic &&
               _duration == other._duration &&
               _participants.SequenceEqual(other._participants) &&
               _publications.SequenceEqual(other._publications);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), _researchTopic, _duration, _participants, _publications);
    }

    public override string ToString()
    {
        string participants = string.Join("\n  ", _participants.Select(p => p.Name));
        string publications = string.Join("\n  ", _publications.Select(p => p.Title));
        return $"ResearchTeam: {Organization}, RegNumber: {RegNumber}, Topic: {_researchTopic}, Duration: {_duration}\n" +
               $"Participants ({_participants.Count}):\n  {participants}\n" +
               $"Publications ({_publications.Count}):\n  {publications}";
    }

    public string ToShortString()
    {
        return $"ResearchTeam: {Organization}, RegNumber: {RegNumber}, Topic: {_researchTopic}, " +
               $"Duration: {_duration}, Participants: {_participants.Count}, Publications: {_publications.Count}";
    }

    public IEnumerable<Person> GetParticipantsWithoutPublications()
    {
        return _participants.Where(p => !_publications.Any(pub => pub.Author.Equals(p)));
    }

    public IEnumerable<Paper> GetPublicationsInLastYears(int years)
    {
        DateTime now = DateTime.Now;
        return _publications.Where(p => (now - p.PublishDate).TotalDays <= years * 365);
    }

    public IEnumerable<Person> GetParticipantsWithMultiplePublications()
    {
        return _participants.Where(p => _publications.Count(pub => pub.Author.Equals(p)) > 1);
    }

    public IEnumerable<Paper> GetPublicationsInLastYear()
    {
        return GetPublicationsInLastYears(1);
    }

    public IEnumerator<Person> GetEnumerator()
    {
        return new ResearchTeamEnumerator(this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public int Compare(ResearchTeam? x, ResearchTeam? y)
    {
        if (x == null && y == null) return 0;
        if (x == null) return -1;
        if (y == null) return 1;
        return string.Compare(x._researchTopic, y._researchTopic, StringComparison.Ordinal);
    }

    private class ResearchTeamEnumerator : IEnumerator<Person>
    {
        private readonly ResearchTeam _researchTeam;
        private int _index = -1;

        public ResearchTeamEnumerator(ResearchTeam researchTeam)
        {
            _researchTeam = researchTeam;
        }

        public Person Current => _researchTeam.Participants[_index];

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            while (++_index < _researchTeam.Participants.Count)
            {
                if (_researchTeam.Publications.Any(p => p.Author.Equals(_researchTeam.Participants[_index])))
                {
                    return true;
                }
            }
            return false;
        }

        public void Reset()
        {
            _index = -1;
        }

        public void Dispose()
        {
        }
    }
}