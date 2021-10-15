using Dom;
using MongoWebApiStarter.Auth;

namespace Migrations;

public class _001_seed_default_roles : IMigration
{
    public async Task UpgradeAsync()
    {
        await DB.DeleteAsync<Role>(_ => true);

        var manager = new Role
        {
            ID = "5ee2298f74057809db6cf5bf",
            SystemRole = true,
            Name = "Manager",
            Permissions = new[] //ONLY THE FOLLOWING:
            {
                    Allow.Account_Read,
                    Allow.Account_Update,
                    Allow.Account_Delete,
                    Allow.Employee_Create,
                    Allow.Employee_Read,
                    Allow.Employee_Update,
                    Allow.Employee_Delete,
                }
        };

        var employee = new Role
        {
            ID = "5ee2298f74057809db6cf5c0",
            SystemRole = true,
            Name = "Employee",
            Permissions = new Allow().Select(x => x.PermissionCode).Except(new[] //EVERYTHING EXCLUDING FOLLOWING:
            {
                    Allow.Employee_Create,
                    Allow.Employee_Delete,
                })
        };

        await DB.SaveAsync(new[] { manager, employee });
    }
}

