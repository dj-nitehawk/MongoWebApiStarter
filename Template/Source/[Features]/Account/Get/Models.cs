﻿using MongoWebApiStarter.Auth;

namespace Account.Get;

public class Request
{
    [From(Claim.AccountID, IsRequired = true)]
    public string AccountID { get; set; }
}

public class Response : Model
{
    public string AccountID { get; set; }
    public bool IsEmailVerified { get; set; }
}