using DotNetCoreIdentityLearn.Helpers.Attributes;

namespace DotNetCoreIdentityLearn.Helpers.Constants;

public enum ApplicationRolesConstants
{
    [Name("Administrator")] 
    [Description("Admins Can (View, Edit, Delete) Information About Users And Managers")]
    Administrator,

    [Name("Manager")]
    [Description("Managers can (View, Edit, Delete) Information About Users But Not Other Managers Or Admins")]
    Manager,

    [Name("User")] 
    [Description("Users Can (View, Edit, Delete) Only Their Own Information.")]
    User
}