    using Lb3.EventHandlers;
    using Lb3.Helpers;

    namespace Lb3;
    public class TeamsJournal
    {
        private List<TeamsJournalEntry> _entries = new List<TeamsJournalEntry>();

        public void HandleEvent(object source, TeamListHandlerEventArgs args)
        {
            Console.WriteLine($"DEBUG [{this.GetHashCode()}]: Received event - {args.ChangeType} at {args.ElementIndex}");
            _entries.Add(new TeamsJournalEntry(args.CollectionName, args.ChangeType, args.ElementIndex));
        }

        public override string ToString()
        {
            return string.Join("\n", _entries.Select(e => e.ToString()));
        }
    }