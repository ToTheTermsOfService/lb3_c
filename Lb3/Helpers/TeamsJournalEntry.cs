namespace Lb3.Helpers;
public class TeamsJournalEntry
{
    public string CollectionName { get; set; }
    public string EventDescription { get; set; }
    public int ElementIndex { get; set; }

    public TeamsJournalEntry(string collectionName, string eventDescription, int elementIndex)
    {
        CollectionName = collectionName;
        EventDescription = eventDescription;
        ElementIndex = elementIndex;
    }

    public override string ToString()
    {
        return $"Journal Entry - Collection: {CollectionName}, Event: {EventDescription}, Index: {ElementIndex}";
    }
}