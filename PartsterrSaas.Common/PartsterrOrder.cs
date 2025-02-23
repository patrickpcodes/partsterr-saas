using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace PartsterrSaas.Common;

public class PartsterrOrder
{
    [JsonProperty("marketplace")]
    public Marketplace Marketplace { get; set; }
    
    [JsonProperty("marketplaceOrderId")]
    public string MarketplaceOrderId { get; set; }
   
    [JsonProperty("orderCreationDate")]
    public DateTimeOffset OrderCreationDate { get; set; }
    
    [JsonProperty("orderDescription")]
    public string Description { get; set; }
    
    [JsonProperty("totalPrice")]
    public string TotalPrice { get; set; }
    [JsonProperty("status")]
    public string Status { get; set; }
    
    public static List<PartsterrOrder> ConvertToPartsterrOrders(List<EbayOrder> ebayOrders)
    {
        return ebayOrders.Select(ebayOrder => new PartsterrOrder
        {
            Marketplace = Marketplace.Ebay,
            MarketplaceOrderId = ebayOrder.OrderId,
            OrderCreationDate = ebayOrder.CreationDate,
            Description = ebayOrder.LineItems.Select(x => x.Title).Aggregate((x, y) => $"{x}, {y}"),
            TotalPrice = ebayOrder.PricingSummary.Total.Value,
            Status = ebayOrder.OrderFulfillmentStatus.ToString()
        }).ToList();
    }
    
}

