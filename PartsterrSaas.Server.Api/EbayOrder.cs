using Newtonsoft.Json;

namespace ParsterrSaas.Server.Api;

public enum EbayOrderFulfillmentStatus
{
    NotStarted,
    InProgress,
    Fulfilled
}

public class EbayOrderFulfillmentStatusConverter : JsonConverter
{
    public override bool CanConvert( Type objectType )
    {
        return objectType == typeof(EbayOrderFulfillmentStatus);
    }

    public override object ReadJson( JsonReader reader, Type objectType, object existingValue,
                                     JsonSerializer serializer )
    {
        string orderStatusString = reader.Value.ToString();
        return orderStatusString switch
        {
            "NOT_STARTED" => EbayOrderFulfillmentStatus.NotStarted,
            "IN_PROGRESS" => EbayOrderFulfillmentStatus.InProgress,
            "FULFILLED" => EbayOrderFulfillmentStatus.Fulfilled,
            _ => throw new ArgumentException( $"Invalid order status string: {orderStatusString}" )
        };
    }

    public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer )
    {
        string orderStatusString = value switch
        {
            EbayOrderFulfillmentStatus.NotStarted => "NOT_STARTED",
            EbayOrderFulfillmentStatus.InProgress => "IN_PROGRESS",
            EbayOrderFulfillmentStatus.Fulfilled => "FULFILLED",
            _ => throw new ArgumentException( $"Invalid order status value: {value}" )
        };
        writer.WriteValue( orderStatusString );
    }
}

public enum EbayOrderPaymentStatus
{
    Failed,
    FullyRefunded,
    Paid,
    PartiallyRefunded,
    Pending
}

public class EbayOrderPaymentStatusConverter : JsonConverter
{
    public override bool CanConvert( Type objectType )
    {
        return objectType == typeof(EbayOrderFulfillmentStatus);
    }

    public override object ReadJson( JsonReader reader, Type objectType, object existingValue,
                                     JsonSerializer serializer )
    {
        string orderPaymentStatus = reader.Value.ToString();
        return orderPaymentStatus switch
        {
            "FAILED" => EbayOrderPaymentStatus.Failed,
            "FULLY_REFUNDED" => EbayOrderPaymentStatus.FullyRefunded,
            "PAID" => EbayOrderPaymentStatus.Paid,
            "PARTIALLY_REFUNDED" => EbayOrderPaymentStatus.PartiallyRefunded,
            "PENDING" => EbayOrderPaymentStatus.Pending,
            _ => throw new ArgumentException( $"Invalid order status string: {orderPaymentStatus}" )
        };
    }

    public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer )
    {
        string orderPaymentStatusString = value switch
        {
            EbayOrderPaymentStatus.Failed => "FAILED",
            EbayOrderPaymentStatus.FullyRefunded => "FULLY_REFUNDED",
            EbayOrderPaymentStatus.Paid => "PAID",
            EbayOrderPaymentStatus.PartiallyRefunded => "PARTIALLY_REFUNDED",
            EbayOrderPaymentStatus.Pending => "PENDING",
            _ => throw new ArgumentException( $"Invalid order status value: {value}" )
        };
        writer.WriteValue( orderPaymentStatusString );
    }
}

public class EbayOrder
{
    // public static int GetSiteId( EbayStoreIds ebayStoreId )
    // {
    //   //switch case for ebayStoreId
    //   return ebayStoreId switch
    //   {
    //     EbayStoreIds.Canada1 or EbayStoreIds.Canada3 or EbayStoreIds.Canada5 or EbayStoreIds.Canada7 or EbayStoreIds.Canada9=> 2,
    //     EbayStoreIds.USA2 or EbayStoreIds.USA4 or EbayStoreIds.USA6 or EbayStoreIds.USA8 or EbayStoreIds.USA10 => 0,
    //     _ => -1,
    //   };
    // }
    //
    // public static bool IsCanada( EbayStoreIds ebayStoreId )
    // {//switch case for ebayStoreId
    //   return ebayStoreId switch
    //   {
    //     EbayStoreIds.Canada1 or EbayStoreIds.Canada3 or EbayStoreIds.Canada5 or EbayStoreIds.Canada7 => true,
    //     EbayStoreIds.USA2 or EbayStoreIds.USA4 or EbayStoreIds.USA6 or EbayStoreIds.USA8 => false,
    //     _ => false,
    //   };
    // }
    // public static string GetCurrency( EbayStoreIds ebayStoreId )
    // {
    //   return IsCanada( ebayStoreId ) ? "CAD" : "USD";
    // }
    // public bool IsShippingToCanada()
    // {
    //   return FulfillmentStartInstructions[0].ShippingStep.ShipTo.ContactAddress.CountryCode switch     {
    //     "CA" => true,
    //     _ => false,
    //   };
    // }
    //
    // public Address GetShipToAddress( int fulfilmentStartInstruction = 0 )
    // {
    //   var contactAddress = FulfillmentStartInstructions[fulfilmentStartInstruction].ShippingStep.ShipTo.ContactAddress;
    //   return new Address( contactAddress.AddressLine1, contactAddress.AddressLine2, contactAddress.City, contactAddress.StateOrProvince, contactAddress.PostalCode, contactAddress.CountryCode );
    // }

    [JsonProperty( "orderId" )]
    public string OrderId { get; set; }

    [JsonProperty( "legacyOrderId" )]
    public string LegacyOrderId { get; set; }

    [JsonProperty( "creationDate" )]
    public DateTime CreationDate { get; set; }

    [JsonProperty( "lastModifiedDate" )]
    public DateTime LastModifiedDate { get; set; }

    [JsonProperty( "orderFulfillmentStatus" )]
    [JsonConverter( typeof(EbayOrderFulfillmentStatusConverter) )]
    public EbayOrderFulfillmentStatus OrderFulfillmentStatus { get; set; }

    [JsonProperty( "orderPaymentStatus" )]
    [JsonConverter( typeof(EbayOrderPaymentStatusConverter) )]
    public EbayOrderPaymentStatus OrderPaymentStatus { get; set; }

    [JsonProperty( "sellerId" )]
    public string SellerId { get; set; }

    [JsonProperty( "buyer" )]
    public Buyer Buyer { get; set; }

    [JsonProperty( "pricingSummary" )]
    public PricingSummary PricingSummary { get; set; }

    [JsonProperty( "cancelStatus" )]
    public CancelStatus CancelStatus { get; set; }

    [JsonProperty( "paymentSummary" )]
    public PaymentSummary PaymentSummary { get; set; }

    [JsonProperty( "fulfillmentStartInstructions" )]
    public List<FulfillmentStartInstruction> FulfillmentStartInstructions { get; set; }

    [JsonProperty( "fulfillmentHrefs" )]
    public List<object> FulfillmentHrefs { get; set; }

    [JsonProperty( "lineItems" )]
    public List<LineItem> LineItems { get; set; }

    [JsonProperty( "ebayCollectAndRemitTax" )]
    public bool EbayCollectAndRemitTax { get; set; }

    [JsonProperty( "salesRecordReference" )]
    public string SalesRecordReference { get; set; }

    [JsonProperty( "totalFeeBasisAmount" )]
    public TotalFeeBasisAmount TotalFeeBasisAmount { get; set; }

    [JsonProperty( "totalMarketplaceFee" )]
    public TotalMarketplaceFee TotalMarketplaceFee { get; set; }
}

public class Amount
{
    [JsonProperty( "value" )]
    public string Value { get; set; }

    [JsonProperty( "currency" )]
    public string Currency { get; set; }
}

public class Buyer
{
    [JsonProperty( "username" )]
    public string Username { get; set; }

    [JsonProperty( "taxAddress" )]
    public TaxAddress TaxAddress { get; set; }
}

public enum EbayOrderCancelState
{
    Canceled,
    InProgress,
    NoneRequested
}

public class EbayOrderCancelStatusConverter : JsonConverter
{
    public override bool CanConvert( Type objectType )
    {
        return objectType == typeof(EbayOrderFulfillmentStatus);
    }

    public override object ReadJson( JsonReader reader, Type objectType, object existingValue,
                                     JsonSerializer serializer )
    {
        string orderStatusString = reader.Value.ToString();
        return orderStatusString switch
        {
            "NONE_REQUESTED" => EbayOrderCancelState.NoneRequested,
            "IN_PROGRESS" => EbayOrderCancelState.InProgress,
            "CANCELED" => EbayOrderCancelState.Canceled,
            _ => throw new ArgumentException( $"Invalid order status string: {orderStatusString}" )
        };
    }

    public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer )
    {
        string orderStatusString = value switch
        {
            EbayOrderCancelState.NoneRequested => "NONE_REQUESTED",
            EbayOrderCancelState.InProgress => "IN_PROGRESS",
            EbayOrderCancelState.Canceled => "CANCELED",
            _ => throw new ArgumentException( $"Invalid order status value: {value}" )
        };
        writer.WriteValue( orderStatusString );
    }
}

public class CancelStatus
{
    [JsonProperty( "cancelState" )]
    [JsonConverter( typeof(EbayOrderCancelStatusConverter) )]
    public EbayOrderCancelState CancelState { get; set; }

    [JsonProperty( "cancelRequests" )]
    public List<object> CancelRequests { get; set; }
}

public class ContactAddress
{
    [JsonProperty( "addressLine1" )]
    public string AddressLine1 { get; set; }

    [JsonProperty( "addressLine2" )]
    public string AddressLine2 { get; set; }

    [JsonProperty( "city" )]
    public string City { get; set; }

    [JsonProperty( "stateOrProvince" )]
    public string StateOrProvince { get; set; }

    [JsonProperty( "postalCode" )]
    public string PostalCode { get; set; }

    [JsonProperty( "countryCode" )]
    public string CountryCode { get; set; }
}

public class DeliveryCost
{
    [JsonProperty( "value" )]
    public string Value { get; set; }

    [JsonProperty( "currency" )]
    public string Currency { get; set; }

    [JsonProperty( "shippingCost" )]
    public ShippingCost ShippingCost { get; set; }
}

public class EbayCollectAndRemitTaxis
{
    [JsonProperty( "taxType" )]
    public string TaxType { get; set; }

    [JsonProperty( "amount" )]
    public Amount Amount { get; set; }

    [JsonProperty( "collectionMethod" )]
    public string CollectionMethod { get; set; }
}

public class FulfillmentStartInstruction
{
    [JsonProperty( "fulfillmentInstructionsType" )]
    public string FulfillmentInstructionsType { get; set; }

    [JsonProperty( "minEstimatedDeliveryDate" )]
    public DateTime MinEstimatedDeliveryDate { get; set; }

    [JsonProperty( "maxEstimatedDeliveryDate" )]
    public DateTime MaxEstimatedDeliveryDate { get; set; }

    [JsonProperty( "ebaySupportedFulfillment" )]
    public bool EbaySupportedFulfillment { get; set; }

    [JsonProperty( "shippingStep" )]
    public ShippingStep ShippingStep { get; set; }
}

public class ItemLocation
{
    [JsonProperty( "location" )]
    public string Location { get; set; }

    [JsonProperty( "countryCode" )]
    public string CountryCode { get; set; }

    [JsonProperty( "postalCode" )]
    public string PostalCode { get; set; }
}

public class LineItem
{
    [JsonProperty( "lineItemId" )]
    public string LineItemId { get; set; }

    [JsonProperty( "legacyItemId" )]
    public string LegacyItemId { get; set; }

    [JsonProperty( "sku" )]
    public string Sku { get; set; }

    [JsonProperty( "title" )]
    public string Title { get; set; }

    [JsonProperty( "lineItemCost" )]
    public LineItemCost LineItemCost { get; set; }

    [JsonProperty( "quantity" )]
    public int Quantity { get; set; }

    [JsonProperty( "soldFormat" )]
    public string SoldFormat { get; set; }

    [JsonProperty( "listingMarketplaceId" )]
    public string ListingMarketplaceId { get; set; }

    [JsonProperty( "purchaseMarketplaceId" )]
    public string PurchaseMarketplaceId { get; set; }

    [JsonProperty( "lineItemFulfillmentStatus" )]
    public string LineItemFulfillmentStatus { get; set; }

    [JsonProperty( "total" )]
    public Total Total { get; set; }

    [JsonProperty( "deliveryCost" )]
    public DeliveryCost DeliveryCost { get; set; }

    [JsonProperty( "appliedPromotions" )]
    public List<object> AppliedPromotions { get; set; }

    [JsonProperty( "taxes" )]
    public List<object> Taxes { get; set; }

    [JsonProperty( "ebayCollectAndRemitTaxes" )]
    public List<EbayCollectAndRemitTaxis> EbayCollectAndRemitTaxes { get; set; }

    [JsonProperty( "properties" )]
    public Properties Properties { get; set; }

    [JsonProperty( "lineItemFulfillmentInstructions" )]
    public LineItemFulfillmentInstructions LineItemFulfillmentInstructions { get; set; }

    [JsonProperty( "itemLocation" )]
    public ItemLocation ItemLocation { get; set; }
}

public class LineItemCost
{
    [JsonProperty( "value" )]
    public string Value { get; set; }

    [JsonProperty( "currency" )]
    public string Currency { get; set; }
}

public class LineItemFulfillmentInstructions
{
    [JsonProperty( "minEstimatedDeliveryDate" )]
    public DateTime MinEstimatedDeliveryDate { get; set; }

    [JsonProperty( "maxEstimatedDeliveryDate" )]
    public DateTime MaxEstimatedDeliveryDate { get; set; }

    [JsonProperty( "shipByDate" )]
    public DateTime ShipByDate { get; set; }

    [JsonProperty( "guaranteedDelivery" )]
    public bool GuaranteedDelivery { get; set; }
}

public class Payment
{
    [JsonProperty( "paymentMethod" )]
    public string PaymentMethod { get; set; }

    [JsonProperty( "paymentReferenceId" )]
    public string PaymentReferenceId { get; set; }

    [JsonProperty( "paymentDate" )]
    public DateTime PaymentDate { get; set; }

    [JsonProperty( "amount" )]
    public Amount Amount { get; set; }

    [JsonProperty( "paymentStatus" )]
    public string PaymentStatus { get; set; }
}

public class PaymentSummary
{
    [JsonProperty( "totalDueSeller" )]
    public TotalDueSeller TotalDueSeller { get; set; }

    [JsonProperty( "refunds" )]
    public List<object> Refunds { get; set; }

    [JsonProperty( "payments" )]
    public List<Payment> Payments { get; set; }
}

public class PriceSubtotal
{
    [JsonProperty( "value" )]
    public string Value { get; set; }

    [JsonProperty( "currency" )]
    public string Currency { get; set; }
}

public class PricingSummary
{
    [JsonProperty( "priceSubtotal" )]
    public PriceSubtotal PriceSubtotal { get; set; }

    [JsonProperty( "deliveryCost" )]
    public DeliveryCost DeliveryCost { get; set; }

    [JsonProperty( "total" )]
    public Total Total { get; set; }
}

public class PrimaryPhone
{
    [JsonProperty( "phoneNumber" )]
    public string PhoneNumber { get; set; }
}

public class Properties
{
    [JsonProperty( "buyerProtection" )]
    public bool BuyerProtection { get; set; }
}

public class ShippingCost
{
    [JsonProperty( "value" )]
    public string Value { get; set; }

    [JsonProperty( "currency" )]
    public string Currency { get; set; }
}

public class ShippingStep
{
    [JsonProperty( "shipTo" )]
    public ShipTo ShipTo { get; set; }

    [JsonProperty( "shippingCarrierCode" )]
    public string ShippingCarrierCode { get; set; }

    [JsonProperty( "shippingServiceCode" )]
    public string ShippingServiceCode { get; set; }
}

public class ShipTo
{
    [JsonProperty( "fullName" )]
    public string FullName { get; set; }

    [JsonProperty( "contactAddress" )]
    public ContactAddress ContactAddress { get; set; }

    [JsonProperty( "primaryPhone" )]
    public PrimaryPhone PrimaryPhone { get; set; }

    [JsonProperty( "email" )]
    public string Email { get; set; }
}

public class TaxAddress
{
    [JsonProperty( "city" )]
    public string City { get; set; }

    [JsonProperty( "stateOrProvince" )]
    public string StateOrProvince { get; set; }

    [JsonProperty( "postalCode" )]
    public string PostalCode { get; set; }

    [JsonProperty( "countryCode" )]
    public string CountryCode { get; set; }
}

public class Total
{
    [JsonProperty( "value" )]
    public string Value { get; set; }

    [JsonProperty( "currency" )]
    public string Currency { get; set; }
}

public class TotalDueSeller
{
    [JsonProperty( "value" )]
    public string Value { get; set; }

    [JsonProperty( "currency" )]
    public string Currency { get; set; }
}

public class TotalFeeBasisAmount
{
    [JsonProperty( "value" )]
    public string Value { get; set; }

    [JsonProperty( "currency" )]
    public string Currency { get; set; }
}

public class TotalMarketplaceFee
{
    [JsonProperty( "value" )]
    public string Value { get; set; }

    [JsonProperty( "currency" )]
    public string Currency { get; set; }
}

public class EbayOrdersResponse
{
    [JsonProperty( "orders" )]
    public List<EbayOrder?> Orders { get; set; }

    [JsonProperty( "href" )]
    public string Href { get; set; }

    [JsonProperty( "total" )]
    public int Total { get; set; }

    [JsonProperty( "prev" )]
    public string Prev { get; set; }

    [JsonProperty( "next" )]
    public string Next { get; set; }

    [JsonProperty( "limit" )]
    public int Limit { get; set; }

    [JsonProperty( "offset" )] 
    public int Offset { get; set; }
}