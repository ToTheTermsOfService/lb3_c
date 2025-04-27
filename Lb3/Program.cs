using Lb3;
using Lb3.Collections;
using Lb3.Helpers;
class Program
{
    static void Main()
    {
        Console.WriteLine("=== Research Team Collection with Events Demo ===");

        var collection1 = new ResearchTeamCollection { CollectionName = "Primary Collection" };
        var collection2 = new ResearchTeamCollection { CollectionName = "Secondary Collection" };

        var journal1 = new TeamsJournal();
        var journal2 = new TeamsJournal();

        Console.WriteLine("\nSetting up event subscriptions...");

        collection1.ResearchTeamAdded += (sender, args) => journal1.HandleEvent(sender, args);
        collection1.ResearchTeamInserted += (sender, args) => journal1.HandleEvent(sender, args);

        collection1.ResearchTeamAdded += journal2.HandleEvent;
        collection1.ResearchTeamInserted += journal2.HandleEvent;
        collection2.ResearchTeamAdded += journal2.HandleEvent;
        collection2.ResearchTeamInserted += journal2.HandleEvent;

        Console.WriteLine("\nMaking changes to collections...");

        Console.WriteLine("\n--- Modifying Primary Collection ---");
        collection1.AddDefaults();

        var aiTeam = new ResearchTeam("AI Lab", 10, "Artificial Intelligence", TimeFrame.TwoYears);
        var bioTeam = new ResearchTeam("Bio Lab", 5, "Biology Research", TimeFrame.Long);

        collection1.AddResearchTeams(aiTeam, bioTeam);
        collection1.InsertAt(1, new ResearchTeam("New Team", 7, "New Research", TimeFrame.Year));
        collection1.InsertAt(10, new ResearchTeam("Out of bounds", 99, "Edge Case", TimeFrame.Long));

        Console.WriteLine("\n--- Modifying Secondary Collection ---");
        collection2.AddResearchTeams(
            new ResearchTeam("OrgA", 1, "TopicA", TimeFrame.Year),
            new ResearchTeam("OrgB", 2, "TopicB", TimeFrame.TwoYears)
        );

        collection2.InsertAt(0, new ResearchTeam("First Position", 3, "Important Research", TimeFrame.Long));
        collection2.InsertAt(2, new ResearchTeam("Middle Position", 4, "Average Research", TimeFrame.Year));

        Console.WriteLine("\n--- Using indexer to modify element ---");
        collection1[0] = new ResearchTeam("Replaced Team", 100, "Replacement Research", TimeFrame.TwoYears);

        Console.WriteLine("\n=== Journal Results ===");

        Console.WriteLine("\nJournal 1 (Primary Collection only):");
        Console.WriteLine(journal1);

        Console.WriteLine("\nJournal 2 (Both collections):");
        Console.WriteLine(journal2);

        Console.WriteLine("\n=== Final Collections State ===");

        Console.WriteLine("\nPrimary Collection:");
        Console.WriteLine(collection1.ToShortList());

        Console.WriteLine("\nSecondary Collection:");
        Console.WriteLine(collection2.ToShortList());

        Console.WriteLine("\nDemo completed. Press any key to exit...");
        Console.ReadKey();
    }
}