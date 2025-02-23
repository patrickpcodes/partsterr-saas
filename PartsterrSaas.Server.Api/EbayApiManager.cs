using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ParsterrSaas.Server.Api;

public class EbayApiManager : IEbayApiManager
{
    private readonly EbaySettings _ebaySettings;

    public EbayApiManager( IOptions<EbaySettings> ebaySettings )
    {
        _ebaySettings = ebaySettings.Value;
    }

    private async Task<EbayAccessToken> GetEbayAccessToken()
    {
        var ebayRefreshTokenSettings = _ebaySettings.EbayRefreshTokens.FirstOrDefault();

        using ( var refreshClient = new HttpClient() )
        {
            refreshClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue( "Basic", ebayRefreshTokenSettings.BasicAuthToken );

            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add( new KeyValuePair<string, string>( "grant_type", ebayRefreshTokenSettings.GrantType ) );
            nvc.Add( new KeyValuePair<string, string>( "refresh_token", ebayRefreshTokenSettings.RefreshToken ) );
            nvc.Add( new KeyValuePair<string, string>( "scope", ebayRefreshTokenSettings.Scope ) );

            var attemps = 10;
            var count = 0;
            while ( count < attemps )
            {
                try
                {
                    var req = new HttpRequestMessage( HttpMethod.Post, _ebaySettings.EbayUrls.TokenUrl )
                        { Content = new FormUrlEncodedContent( nvc ) };

                    var res = await refreshClient.SendAsync( req );

                    var temp2 = await res.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<EbayAccessToken>( temp2 );
                }
                catch ( Exception e )
                {
                }

                count++;
            }

            throw new NotImplementedException();
        }
    }


    public async Task<List<EbayOrder>> GetAllEbayOrdersAfterDate()
    {
        var ebayAccessToken = await GetEbayAccessToken();
        if ( ebayAccessToken == null )
            return new List<EbayOrder>();
        using ( var httpClient = new HttpClient() )
        {
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue( "Bearer", ebayAccessToken.AccessToken );
            var limit = 200;
            var offset = 0;

            var reachedEnd = false;

            List<EbayOrder> ebayOrders = new List<EbayOrder>();

            while ( !reachedEnd )
            {
                var response =
                    await httpClient.GetStringAsync(
                        _ebaySettings.EbayUrls.OrderUrl + $"?limit={limit}&offset={offset}" );

                var ebayOrderResponse = JsonConvert.DeserializeObject<EbayOrdersResponse>( response );

                if ( ebayOrderResponse != null && ebayOrderResponse.Orders != null &&
                     ebayOrderResponse.Orders.Count > 0 )
                {
                    //ebayOrders = ebayOrderResponse.Orders;

                    if ( ebayOrderResponse.Orders?.Count < limit )
                        reachedEnd = true;

                    var ordersGreaterThenMinDate = ebayOrderResponse.Orders.ToList();

                    if ( ordersGreaterThenMinDate.Count < ebayOrderResponse.Orders?.Count )
                    {
                        reachedEnd = true;
                    }
                    else
                    {
                        offset += limit;
                    }

                    ebayOrders.AddRange( ordersGreaterThenMinDate );
                    if ( ebayOrders.Count > 50 )
                        break;
                }
                else
                {
                    break;
                }
            }

            return ebayOrders;
        }
    }
}

public interface IEbayApiManager
{
    Task<List<EbayOrder>> GetAllEbayOrdersAfterDate();
}