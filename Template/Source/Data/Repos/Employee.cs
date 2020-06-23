using MongoDB.Entities;
using System;

namespace Data
{
    public class RepoEmployee : RepoBase<Employee>
    {
        static RepoEmployee()
        {
            DB.Index<Employee>()
              .Key(e => e.VP.ID, KeyType.Ascending)
              .CreateAsync();

            DB.Index<Employee>()
              .Key(e => e.UserName, KeyType.Ascending)
              .CreateAsync();
        }

        public static void SetSignUpCode(string code, string employeeID)
        {
            DB.Update<Employee>()
              .Match(e => e.ID == employeeID)
              .Modify(e => e.SignUpCode, code)
              .Modify(e => e.SignUpCodeExpiry, DateTime.UtcNow)
              .Execute();
        }

        public static void SetCredentials(string id, string username, string passwordHash)
        {
            DB.Update<Employee>()
              .Match(e => e.ID == id)
              .Modify(e => e.UserName, username)
              .Modify(e => e.PasswordHash, passwordHash)
              .Modify(b => b.Unset(e => e.SignUpCode))
              .Modify(b => b.Unset(e => e.SignUpCodeExpiry))
              .Execute();
        }
    }
}
