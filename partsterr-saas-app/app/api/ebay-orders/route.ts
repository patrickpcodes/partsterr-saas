// app/api/ebay-orders/route.ts
import { NextRequest, NextResponse } from "next/server";
import { getAllEbayOrdersAfterDate } from "@/lib/ebayApi"; // adjust path as needed

// You’ll want to inject these from .env or some config
const ebaySettings = {
  EbayRefreshTokens: [
    {
      BasicAuthToken: process.env.EBAY_BASIC_AUTH_TOKEN || "",
      GrantType: "refresh_token",
      RefreshToken: process.env.EBAY_REFRESH_TOKEN || "",
      Scope: "https://api.ebay.com/oauth/api_scope/sell.fulfillment",
    },
  ],
  EbayUrls: {
    TokenUrl: "https://api.ebay.com/identity/v1/oauth2/token",
    OrderUrl: "https://api.ebay.com/sell/fulfillment/v1/order",
  },
};

// For a GET request to /api/ebay-orders
export async function GET(req: NextRequest) {
  try {
    const orders = await getAllEbayOrdersAfterDate(ebaySettings);

    return NextResponse.json(orders, { status: 200 });
  } catch (err) {
    console.error("Error retrieving eBay orders:", err);
    return NextResponse.json(
      { message: "Failed to retrieve eBay orders." },
      { status: 500 }
    );
  }
}
