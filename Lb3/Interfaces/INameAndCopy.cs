namespace Lb3.Interfaces;
interface INameAndCopy
{
    string Name { get; set; }
    object DeepCopy();
}