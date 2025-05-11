using Lb3.Interfaces;

namespace Lb3;
[Serializable]
public class Paper
{
    public Paper() : this("Курсова", new Person(), DateTime.Now)
    {
    }

    public Paper(string title, Person author, DateTime publicationDate)
    {
        Title = title;
        Author = author;
        PublicationDate = publicationDate;
    }

    public string Title { get; init; }
    public Person Author { get; init; }
    public DateTime PublicationDate { get; init; }

    public virtual object DeepCopy()
    {
        return new Paper(
            Title,
            (Person)Author.DeepCopy(),
            PublicationDate
        );
    }

    public override string ToString()
    {
        return $"'{Title}' написав(-ла) {Author.ToShortString()}, опублiковано {PublicationDate:d}";
    }
}