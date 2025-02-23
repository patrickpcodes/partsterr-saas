// ebayApi.ts
interface EbayRefreshTokenSettings {
  BasicAuthToken: string; // Base64-encoded client_id:client_secret
  GrantType: string; // Typically "refresh_token"
  RefreshToken: string;
  Scope: string;
}

interface EbayUrls {
  TokenUrl: string; // e.g. "https://api.ebay.com/identity/v1/oauth2/token"
  OrderUrl: string; // e.g. "https://api.ebay.com/sell/fulfillment/v1/order"
}

interface EbaySettings {
  EbayRefreshTokens: EbayRefreshTokenSettings[];
  EbayUrls: EbayUrls;
}

// The response from eBay when we get an access token.
interface EbayAccessToken {
  access_token: string;
  token_type?: string;
  expires_in?: number;
  refresh_token?: string;
  // Add any other fields as needed.
}

export interface EbayOrder {
  orderId: string;
  pricingSummary: {
    priceSubtotal: {
      value: string;
      currency: string;
    };
    deliveryCost: {
      value: string;
      currency: string;
    };
    total: {
      value: string;
      currency: string;
    };
  };
}

// 2. Write a small transform function
export function parseEbayOrder(raw: any): EbayOrder {
  // If you're sure the JSON is valid and contains these fields,
  // you can do direct assignment. Otherwise, add safety checks as needed.
  return {
    orderId: raw.orderId,
    pricingSummary: {
      priceSubtotal: {
        value: raw.pricingSummary?.priceSubtotal.value ?? "",
        currency: raw.pricingSummary?.priceSubtotal.currency ?? "",
      },
      deliveryCost: {
        value: raw.pricingSummary?.deliveryCost.value ?? "",
        currency: raw.pricingSummary?.deliveryCost.currency ?? "",
      },
      total: {
        value: raw.pricingSummary?.total.value ?? "",
        currency: raw.pricingSummary?.total.currency ?? "",
      },
    },
  };
}

// The JSON response from eBay when retrieving orders.
interface EbayOrdersResponse {
  orders: EbayOrder[];
  // eBay might include additional fields like 'href', 'total', etc.
}

export async function getEbayAccessToken(
  ebaySettings: EbaySettings
): Promise<EbayAccessToken> {
  const ebayRefreshTokenSettings = ebaySettings.EbayRefreshTokens[0];
  if (!ebayRefreshTokenSettings) {
    throw new Error("No eBay refresh token settings found.");
  }

  const attempts = 10;
  let count = 0;

  while (count < attempts) {
    try {
      // Build form data for the POST request
      const bodyParams = new URLSearchParams();
      bodyParams.append("grant_type", ebayRefreshTokenSettings.GrantType);
      bodyParams.append("refresh_token", ebayRefreshTokenSettings.RefreshToken);
      bodyParams.append("scope", ebayRefreshTokenSettings.Scope);

      // Perform the token refresh request
      const response = await fetch(ebaySettings.EbayUrls.TokenUrl, {
        method: "POST",
        headers: {
          Authorization: `Basic ${ebayRefreshTokenSettings.BasicAuthToken}`,
          "Content-Type": "application/x-www-form-urlencoded",
        },
        body: bodyParams.toString(),
      });

      if (!response.ok) {
        // Not successful – throw or just let it fall into the catch
        throw new Error(
          `Token request failed with status ${response.status} ${response.statusText}`
        );
      }

      // Parse the JSON response
      const tokenData = (await response.json()) as EbayAccessToken;
      return tokenData;
    } catch (error) {
      console.error("Error fetching eBay access token:", error);
      // We’ll retry up to `attempts` times
    }

    count++;
  }

  // If we exhaust our attempts, throw an error
  throw new Error(
    "Failed to retrieve eBay access token after multiple attempts."
  );
}

export async function getAllEbayOrdersAfterDate(
  ebaySettings: EbaySettings
): Promise<EbayOrder[]> {
  try {
    // First, get the access token
    const ebayAccessToken = await getEbayAccessToken(ebaySettings);
    console.log("eBay access token:", ebayAccessToken);
    // If we failed for some reason (null check), bail out
    if (!ebayAccessToken?.access_token) {
      return [];
    }

    // Let's define our pagination variables
    const limit = 200;
    let offset = 0;
    let reachedEnd = false;

    const ebayOrders: EbayOrder[] = [];

    // We’ll keep calling the eBay orders API until we reach the end
    while (!reachedEnd) {
      const url = `${ebaySettings.EbayUrls.OrderUrl}?limit=${limit}&offset=${offset}`;

      const response = await fetch(url, {
        headers: {
          Authorization: `Bearer ${ebayAccessToken.access_token}`,
        },
      });

      if (!response.ok) {
        console.error("Failed to fetch eBay orders:", await response.text());
        break;
      }

      const data = (await response.json()) as EbayOrdersResponse;

      if (data?.orders && data.orders.length > 0) {
        // If we got fewer orders than `limit`, we've probably hit the end
        if (data.orders.length < limit) {
          reachedEnd = true;
        }

        const ordersForThisPage = data.orders;
        ebayOrders.push(...ordersForThisPage);

        // const transformedOrders = data.orders.map((rawOrder) =>
        //   parseEbayOrder(rawOrder)
        // );
        // ebayOrders.push(...transformedOrders);

        // Move on to the next page
        offset += limit;

        // If you have a max count (like 50 in the C# code):
        if (ebayOrders.length > 50) {
          break;
        }
      } else {
        // No orders -> end the loop
        break;
      }
    }

    return ebayOrders;
  } catch (err) {
    console.error("Error in getAllEbayOrdersAfterDate:", err);
    return [];
  }
}
