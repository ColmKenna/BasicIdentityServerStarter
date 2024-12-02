namespace IdentityServer.EF.DataAccess;

public class UserClaim
{
    public string ClaimType { get; set; }
    public string? ClaimValue { get; set; }
    
    // equals
    public override bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }

        if (obj.GetType() != typeof(UserClaim))
        {
            return false;
        }

        var userClaim = (UserClaim) obj;
        return userClaim.ClaimType == ClaimType && userClaim.ClaimValue == ClaimValue;
    }
    
    public override int GetHashCode()
    {
        return (ClaimType + ClaimValue).GetHashCode();
    }
    public static bool operator ==(UserClaim left, UserClaim right)
    {
        return left.Equals(right);
    }
    public static bool operator !=(UserClaim left, UserClaim right)
    {
        return !left.Equals(right);
    }
    
    
}