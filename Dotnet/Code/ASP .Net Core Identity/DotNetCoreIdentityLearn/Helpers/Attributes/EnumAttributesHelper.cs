namespace DotNetCoreIdentityLearn.Helpers.Attributes;

public static class EnumAttributesHelper
{
    /// <summary>
    /// Gets an attribute on an enum field value
    /// </summary>
    /// <typeparam name="T">The type of the attribute you want to retrieve</typeparam>
    /// <param name="enumVal">The enum value</param>
    /// <returns>The attribute of type T that exists on the enum value</returns>
    /// <example><![CDATA[string desc = myEnumVariable.GetAttributeOfType<DescriptionAttribute>().Description;]]></example>
    private static T? GetAttributeOfType<T>(this Enum enumVal) where T : Attribute
    {
        var type = enumVal.GetType();
        var memInfo = type.GetMember(enumVal.ToString());
        var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
        return (attributes.Length > 0) ? (T)attributes[0] : null;
    }

    public static string GetName(this Enum enumVal)
    {
        return enumVal.GetAttributeOfType<NameAttribute>()?.Name ?? "";
    }

    public static string GetDescription(this Enum enumVal)
    {
        return enumVal.GetAttributeOfType<DescriptionAttribute>()?.Description ?? "";
    }
}