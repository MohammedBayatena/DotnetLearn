namespace DotNetCoreIdentityLearn.Helpers.Attributes;

[AttributeUsage(AttributeTargets.Field)]
public class DescriptionAttribute(string description) : Attribute
{
    public string Description { get; } = description;
}