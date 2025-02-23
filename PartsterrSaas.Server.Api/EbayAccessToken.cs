using Newtonsoft.Json;

namespace ParsterrSaas.Server.Api;
public class EbayAccessToken
{
    [JsonProperty( "access_token" )]
    public string AccessToken { get; set; }

    [JsonProperty( "expires_in" )]
    public int ExpiresIn { get; set; }

    [JsonProperty( "token_type" )]
    public string TokenType { get; set; }
}
public class EbayAuthorizationAccessToken
{
    [JsonProperty( "access_token" )]
    public string AccessToken { get; set; }

    [JsonProperty( "expires_in" )]
    public int ExpiresIn { get; set; }

    [JsonProperty( "token_type" )]
    public string TokenType { get; set; }
    [JsonProperty( "refresh_token" )]
    public string RefreshToken { get; set; }
    [JsonProperty( "refresh_token_expires_in" )]
    public int RefreshTokenExpiresIn { get; set; }
}