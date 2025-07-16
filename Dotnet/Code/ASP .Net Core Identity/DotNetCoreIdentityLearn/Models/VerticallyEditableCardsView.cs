namespace DotNetCoreIdentityLearn.Models;

public class VerticallyEditableCardsView<T>
    where T : class
{
    public required IEnumerable<T> Items { get; set; }
}