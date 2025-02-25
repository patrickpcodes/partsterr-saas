// app/api/ebay-orders/route.ts
import { NextRequest, NextResponse } from "next/server";
import { getAllEbayOrdersAfterDate } from "@/lib/ebayApi"; // adjust path as needed

// For a GET request to /api/ebay-orders
export async function GET(req: NextRequest) {
  try {
    const orders = await getAllEbayOrdersAfterDate();

    return NextResponse.json(orders, { status: 200 });
  } catch (err) {
    console.error("Error retrieving eBay orders:", err);
    return NextResponse.json(
      { message: "Failed to retrieve eBay orders." },
      { status: 500 }
    );
  }
}
