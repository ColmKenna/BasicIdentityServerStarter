namespace IdentityDataModels;

public class ApplicationUserDetails
{

    public ApplicationUserDetails()
    {
        UserClaims = new List<UserClaimSummary>();
        Roles = new List<string>();
    }

    public DateTimeOffset? LockoutEnd { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public bool PhoneNumberConfirmed { get; set; }
    public string? PhoneNumber { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Id { get; set; }
    public bool LockoutEnabled{ get; set; }
    public int AccessFailedCount { get; set; }


    public IList<UserClaimSummary> UserClaims { get; set; }

    public IList<string> Roles { get; set; }


    public ApplicationUserDetails SetUserClaims(IList<UserClaimSummary> userClaimSummaries)
    {
        this.UserClaims = userClaimSummaries;
        foreach (var userClaimSummary in UserClaims)
        {
            userClaimSummary.UserId = this.Id;
        }
        return this;
    }
    public ApplicationUserDetails SetUserRoles(IList<string> userRoles)
    {
        this.Roles = userRoles;
        return this;
    }
}