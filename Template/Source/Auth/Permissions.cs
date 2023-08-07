namespace MongoWebApiStarter.Auth;

/// <summary>
/// The allowed operations/permissions for the application
/// </summary>
internal sealed class Allow : Permissions
{
    //NOTE: these permission names are sent to the UI with the login response in a field called PermissionSet[]. 
    //      if they change; UI code needs to change too.
    //      values are only added to the jwt token to enable authorization attributes on every request.

    //ACCOUNT
    public const string Account_Read = "100";
    public const string Account_Update = "101";
    public const string Account_Delete = "102";

    //EMPLOYEE
    public const string Employee_Create = "200";
    public const string Employee_Read = "201";
    public const string Employee_Update = "202";
    public const string Employee_Delete = "203";

}

