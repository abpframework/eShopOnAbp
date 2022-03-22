namespace EShopOnAbp.PublicWeb.Options
{
    public class RemoteServices
    {
        public Default Default { get; set; } = new();
    }

    public class Default
    {
        public string BaseUrl { get; set; }
    }
}


