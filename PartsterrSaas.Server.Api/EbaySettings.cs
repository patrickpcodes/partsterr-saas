namespace ParsterrSaas.Server.Api;

public class EbaySettings
{
    public EbayUrls EbayUrls { get; set; }

    public List<EbayRefreshTokenSettings> EbayRefreshTokens { get; set; } 
}

public class EbayUrls
{
    public string TokenUrl { get; set; }
    //Break up scopes, define list and get it to work
    public string FulfillmentScopeUrl { get; set; }
    public string OrderUrl { get; set; }
    public string InventoryUrl { get; set; }
}

public class EbayRefreshTokenSettings
{
    // public EbayStoreIds EbayStoreId { get; set; }

    public string BasicAuthToken { get; set; }

    public string GrantType { get; set; }

    public string RefreshToken { get; set; }

    public string Scope { get; set; }
    public string EbayUsername { get; set; }
}