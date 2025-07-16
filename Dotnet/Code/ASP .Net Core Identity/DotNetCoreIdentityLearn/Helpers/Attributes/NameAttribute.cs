namespace DotNetCoreIdentityLearn.Helpers.Attributes;

[AttributeUsage(AttributeTargets.Field)]
public class NameAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}