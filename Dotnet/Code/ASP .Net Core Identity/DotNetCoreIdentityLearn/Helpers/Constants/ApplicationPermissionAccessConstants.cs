using DotNetCoreIdentityLearn.Helpers.Attributes;

namespace DotNetCoreIdentityLearn.Helpers.Constants;

public enum ApplicationPermissionAccessConstants
{
    [Name("Read")] [Description("Allows Read Only")]
    Read = 0,

    [Name("Limited")] [Description("Allows Read and Create Only")]
    Limited = 1,

    [Name("Full")] [Description("Allows Read, Create, Update , Delete")]
    Full = 2,

    [Name("Custom")] [Description("Custom Permissions")]
    Custom = 3
}