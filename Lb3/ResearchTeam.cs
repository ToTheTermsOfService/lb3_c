using Lb3;
using Lb3.Helpers;
using System.Collections;
using System.Text.Json;

[Serializable]
public class ResearchTeam : Team, IComparable<ResearchTeam>, IComparer<ResearchTeam>
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        WriteIndented = true,
        ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve
    };

    private string _topic;
    private TimeFrame _timeFrame;
    private List<Person> _participants;
    private List<Paper?> _publications;

    public ResearchTeam(string organization, int registrationNumber, string topic, TimeFrame timeFrame)
        : base(organization, registrationNumber)
    {
        Topic = topic;
        TimeFrame = timeFrame;
        Participants = [];
        Publications = [];
    }

    public ResearchTeam() : this("Стандартна організація", 1, "Стандартна тема", TimeFrame.Year)
    {
    }

    public string Topic
    {
        get => _topic;
        set => _topic = value;
    }

    public TimeFrame TimeFrame
    {
        get => _timeFrame;
        set => _timeFrame = value;
    }

    public List<Person> Participants
    {
        get => _participants;
        set => _participants = value;
    }

    public List<Paper?> Publications
    {
        get => _publications;
        set => _publications = value;
    }

    public Team Team => new Team(Organization, RegistrationNumber);

    public void AddParticipants(params Person[] persons)
    {
        Participants.AddRange(persons);
    }

    public void AddPapers(params Paper[] papers)
    {
        if (papers == null || papers.Length == 0) return;
        Publications.AddRange(papers);
    }

    public Paper? LatestPublication
    {
        get
        {
            return Publications.Count == 0 ? null : Publications.OrderByDescending(p => p?.PublicationDate).First();
        }
    }

    public bool this[TimeFrame timeFrame] => TimeFrame == timeFrame;

    public new object DeepCopy()
    {
        try
        {
            var jsonString = JsonSerializer.Serialize(this, JsonSerializerOptions);
            return JsonSerializer.Deserialize<ResearchTeam>(jsonString, JsonSerializerOptions)!;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating deep copy: {ex.Message}");
            throw;
        }
    }

    public bool Save(string? filename)
    {
        if (filename is null) return false;
        try
        {
            var jsonString = JsonSerializer.Serialize(this, JsonSerializerOptions);
            File.WriteAllText(filename, jsonString);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving to file: {ex.Message}");
            return false;
        }
    }

    public bool Load(string? filename)
    {
        if (filename is null) return false;
        if (!File.Exists(filename))
        {
            Console.WriteLine("File does not exist!");
            return false;
        }

        try
        {
            var jsonString = File.ReadAllText(filename);

            var temp = JsonSerializer.Deserialize<ResearchTeam>(jsonString, JsonSerializerOptions);

            if (temp == null) return false;

            Organization = temp.Organization;
            RegistrationNumber = temp.RegistrationNumber;
            Topic = temp.Topic;
            TimeFrame = temp.TimeFrame;

            Participants.Clear();
            foreach (var person in temp.Participants)
            {
                Participants.Add(person);
            }

            Publications.Clear();
            foreach (var paper in temp.Publications.OfType<Paper>())
            {
                Publications.Add(paper);
            }

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading from file: {ex.Message}");
            return false;
        }
    }

    public bool AddFromConsole()
    {
        Console.WriteLine("Додавання нової публікації");
        Console.WriteLine("Введіть дані у форматі: 'Назва публікації;Ім'я автора;Прізвище автора;Дата народження (DD.MM.YYYY);Дата публікації (DD.MM.YYYY)'");
        Console.WriteLine("Приклад: 'Наукова стаття;Іван;Петренко;01.01.1990;15.06.2023'");

        var input = Console.ReadLine();

        try
        {
            var parts = input?.Split(';');
            if (parts != null && parts.Length != 5)
            {
                throw new FormatException("Неправильна кількість параметрів");
            }

            if (parts != null)
            {
                var title = parts[0].Trim();
                var firstName = parts[1].Trim();
                var lastName = parts[2].Trim();
                var birthDate = DateTime.Parse(parts[3].Trim());
                var publicationDate = DateTime.Parse(parts[4].Trim());

                var author = new Person(firstName, lastName, birthDate);
                var paper = new Paper(title, author, publicationDate);

                Publications.Add(paper);
            }

            Console.WriteLine("Публікацію успішно додано!");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка при додаванні публікації: {ex.Message}");
            return false;
        }
    }

    public static bool Save(string? filename, ResearchTeam? team)
    {
        return team != null && team.Save(filename);
    }

    public static bool Load(string? filename, ResearchTeam? team)
    {
        return team is not null && team.Load(filename);
    }

    public override string ToString()
    {
        var result = $"{base.ToString()}, Тема: {Topic}, Тривалість: {TimeFrame}\n";

        result += "Учасники:\n";
        if (Participants.Count > 0)
        {
            foreach (Person person in Participants)
            {
                result += $"- {person}\n";
            }
        }
        else
        {
            result += "- Немає учасників\n";
        }

        result += "Публікації:\n";
        if (Publications.Count > 0)
        {
            foreach (Paper? paper in Publications)
            {
                result += $"- {paper}\n";
            }
        }
        else
        {
            result += "- Немає публікацій\n";
        }

        return result;
    }

    public virtual string ToShortString()
    {
        return $"{base.ToString()}, Тема: {Topic}, Тривалість: {TimeFrame}, Кількість учасників: {Participants.Count}, Кількість публікацій: {Publications.Count}";
    }

    public int CompareTo(ResearchTeam? other)
    {
        return other == null ? 1 : RegistrationNumber.CompareTo(other.RegistrationNumber);
    }

    public int Compare(ResearchTeam? x, ResearchTeam? y)
    {
        if (x == null && y == null) return 0;
        if (x == null) return -1;
        if (y == null) return 1;

        return string.CompareOrdinal(x.Topic, y.Topic);
    }
}