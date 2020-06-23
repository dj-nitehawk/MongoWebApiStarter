using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Entities;
using SCVault.Tests;
using ServiceStack;
using System;

namespace SCVault.Tests_Main
{
    [TestClass]
    public class Account
    {
        private static readonly JsonServiceClient client = new JsonServiceClient(Init.BaseUrl);

        [TestMethod]
        public void Create()
        {
            var guid = Guid.NewGuid().ToString();

            var req = new Main.Account.Save.Request
            {
                City = "city",
                CountryCode = "LKA",
                EmailAddress = $"{guid}@email.com",
                FirstName = "firstname",
                LastName = "surname",
                Mobile = "0713429292",
                Password = "qqqqq123Q",
                State = "state",
                Street = "street",
                TimeZoneOffsetHrs = 5.5m,
                Title = "mr.",
                VPName = "my vp",
                ZipCode = "10100"
            };

            var res = client.Post(req);

            res.EmailSent.Should().BeTrue();
            res.ID.Should().NotBeNullOrEmpty();

            var acc = DB.Find<Data.Account>().One(res.ID);

            acc.Email.Should().BeEquivalentTo(req.EmailAddress);
            acc.IsEmailVerified.Should().BeFalse();
        }
    }
}
