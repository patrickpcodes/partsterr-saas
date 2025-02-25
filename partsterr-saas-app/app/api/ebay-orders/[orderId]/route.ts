// app/api/ebay-orders/[orderId]/route.ts

import { NextRequest, NextResponse } from "next/server";
import { getEbayOrderById } from "@/lib/ebayApi"; // Adjust path to match your setup

export async function GET(
  request: NextRequest,
  { params }: { params: Promise<{ orderId: string }> }
) {
  try {
    const { orderId } = await params;
    if (!orderId) {
      return NextResponse.json(
        { message: "orderId is required" },
        { status: 400 }
      );
    }

    const order = await getEbayOrderById(orderId);

    if (!order) {
      // Could be that eBay returned 404 or parse failed
      return NextResponse.json(
        { message: `Order ${orderId} not found or could not be retrieved.` },
        { status: 404 }
      );
    }

    return NextResponse.json(order, { status: 200 });
  } catch (err) {
    console.error("Error retrieving eBay order:", err);
    return NextResponse.json(
      { message: "Failed to retrieve eBay order." },
      { status: 500 }
    );
  }
}
