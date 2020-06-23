namespace MongoWebApiStarter.Auth
{
    /// <summary>
    /// The allowed operations/permissions for the application
    /// </summary>
    public enum Allow : ushort
    {
        //NOTE: these permission names are sent to the UI with the login response in a field called PermissionSet[]. 
        //      if they change, UI code needs to change too.
        //      values are only added to the jwt token to enable authorization attributes on every request.

        //ACCOUNT
        Account_Read = 1,
        Account_Update = 2,
        Account_Delete = 3,

        //EMPLOYEE
        Employee_Create = 100,
        Employee_Read = 101,
        Employee_Update = 102,
        Employee_Delete = 103,

    }
}
