namespace IdentityDataModels;

public class ClientsSummary
{

    public int Id { get; set; }
    public string ClientId { get; set; }
    public string ClientName { get; set; }
    public string Description { get; set; }
    public IList<string> RedirectUris { get; set; }

    protected bool Equals(ClientsSummary other)
    {
        return Id == other.Id && 
               ClientName == other.ClientName && 
               ClientId == other.ClientId &&
               Description == other.Description;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((ClientsSummary)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, ClientName, Description);
    }



}