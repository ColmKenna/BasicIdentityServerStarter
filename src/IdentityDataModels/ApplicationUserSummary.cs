﻿namespace IdentityDataModels;

public class ApplicationUserSummary
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public IList<string> UserClaims { get; set; }


}