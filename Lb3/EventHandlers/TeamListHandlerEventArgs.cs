namespace Lb3.EventHandlers;
public class TeamListHandlerEventArgs : EventArgs
{
    public string CollectionName { get; set; }
    public string ChangeType { get; set; }
    public int ElementIndex { get; set; }

    public TeamListHandlerEventArgs(string collectionName, string changeType, int elementIndex)
    {
        CollectionName = collectionName;
        ChangeType = changeType;
        ElementIndex = elementIndex;
    }

    public override string ToString()
    {
        return $"Collection: {CollectionName}, Change: {ChangeType}, Index: {ElementIndex}";
    }
}