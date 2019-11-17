using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoWebApiStarter.Biz.Auth;
using FluentAssertions;

namespace MongoWebApiStarter.Test
{
    [TestClass]
    public class RolesTest
    {
        [TestMethod]
        public void role_dictionary_is_not_empty()
        {
            Assert.IsTrue(Roles.AllRoles.Count > 0);
        }

        [TestMethod]
        public void get_role_names_from_values()
        {
            var vals = new[] { Roles.Admin, Roles.Employee };
            var names = Roles.GetRoleNames(vals);

            names.Length.Should().Be(2);
            names[0].Should().Be(nameof(Roles.Admin));
        }

        [TestMethod]
        public void get_role_values_from_names()
        {
            var names = new[] { Roles.GetRoleName(Roles.Admin), Roles.GetRoleName(Roles.Employee) };
            var vals = Roles.GetRoleValues(names);

            vals.Length.Should().Be(2);
            vals[0].Should().Be(Roles.Admin);
        }
    }
}
