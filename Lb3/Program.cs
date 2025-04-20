using Lb3;
using Lb3.Collections;
using Lb3.Helpers;
class Program
{
    static void Main()
    {
        ResearchTeamCollection collection = new ResearchTeamCollection();
        collection.AddDefaults();

        ResearchTeam rt1 = new ResearchTeam("AI Lab", 10, "Artificial Intelligence", TimeFrame.TwoYears);
        rt1.AddParticipants(new Person { FirstName = "John", LastName = "Doe", BirthDate = new DateTime(1980, 1, 1) });
        rt1.AddPapers(new Paper { Title = "AI Advances", Author = new Person { FirstName = "John", LastName = "Doe", BirthDate = new DateTime(1980, 1, 1) }, PublishDate = DateTime.Now });

        ResearchTeam rt2 = new ResearchTeam("Bio Lab", 5, "Biology Research", TimeFrame.Long);
        rt2.AddParticipants(
            new Person { FirstName = "Alice", LastName = "Smith", BirthDate = new DateTime(1975, 5, 5) },
            new Person { FirstName = "Bob", LastName = "Johnson", BirthDate = new DateTime(1985, 10, 10) }
        );
        rt2.AddPapers(
            new Paper { Title = "Cell Biology", Author = new Person { FirstName = "Alice", LastName = "Smith", BirthDate = new DateTime(1975, 5, 5) }, PublishDate = DateTime.Now.AddMonths(-6) },
            new Paper { Title = "Genetics", Author = new Person { FirstName = "Alice", LastName = "Smith", BirthDate = new DateTime(1975, 5, 5) }, PublishDate = DateTime.Now.AddMonths(-3) }
        );

        collection.AddResearchTeams(rt1, rt2);

        Console.WriteLine("\nFull collection:");
        Console.WriteLine(collection.ToString());

        Console.WriteLine("\nShort info:");
        Console.WriteLine(collection.ToShortList());

        Console.WriteLine("\nSorted by registration number:");
        collection.SortByRegNumber();
        Console.WriteLine(collection.ToShortList());

        Console.WriteLine("\nSorted by research topic:");
        collection.SortByResearchTopic();
        Console.WriteLine(collection.ToShortList());

        Console.WriteLine("\nSorted by publications count:");
        collection.SortByPublicationsCount();
        Console.WriteLine(collection.ToShortList());

        Console.WriteLine($"\nMin registration number: {collection.MinRegNumber}");

        Console.WriteLine("\nTeams with TwoYears duration:");
        foreach (var team in collection.TwoYearsDurationTeams)
        {
            Console.WriteLine(team.ToShortString());
        }

        Console.WriteLine("\nTeams grouped by participant count:");
        var grouped = collection.NGroup(2);
        foreach (var team in grouped)
        {
            Console.WriteLine(team.ToShortString());
        }

        Console.WriteLine("\n=== Performance Comparison (Lab 4) ===");
        Console.Write("Enter number of elements for performance test: ");
        int count;
        while (!int.TryParse(Console.ReadLine(), out count) || count <= 0)
        {
            Console.WriteLine("Invalid input. Please enter a positive integer.");
            Console.Write("Enter number of elements for performance test: ");
        }

        // Тестування стандартних колекцій
        Console.WriteLine("\n=== Standard Collections ===");
        TestCollections standardTest = new TestCollections(count);
        standardTest.MeasureSearchTime();

        // Тестування immutable колекцій
        Console.WriteLine("\n=== Immutable Collections ===");
        ImmutableTestCollections immutableTest = new ImmutableTestCollections(count);
        immutableTest.MeasureSearchTime();

        // Тестування sorted колекцій
        Console.WriteLine("\n=== Sorted Collections ===");
        SortedTestCollections sortedTest = new SortedTestCollections(count);
        sortedTest.MeasureSearchTime();

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}