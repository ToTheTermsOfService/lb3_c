using Lb3.Interfaces;

namespace Lb3;
class Paper : INameAndCopy
{
    public required string Title { get; set; }
    public required Person Author { get; set; }
    public DateTime PublishDate { get; set; }
    public string Name { get => Title; set { } }

    public virtual object DeepCopy()
    {
        return new Paper { Title = Title, Author = (Person)Author.DeepCopy(), PublishDate = PublishDate };
    }
}
