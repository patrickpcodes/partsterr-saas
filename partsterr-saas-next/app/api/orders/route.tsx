import { PartsterrOrder } from "@/lib/types/Order";
import { NextResponse } from "next/server";

// Disable SSL certificate validation for local development
process.env.NODE_TLS_REJECT_UNAUTHORIZED = "0";

export async function GET() {
  try {
    const response = await fetch("https://localhost:7142/Ebay", {
      method: "GET", // Ensure the method is GET
    });
    const orders = await response.json();
    //convert into PartsterrOrder type
    const partsterrOrders = orders.map((order: PartsterrOrder) => {
      return {
        marketplace: order.marketplace,
        description: order.description,
        marketplaceOrderId: order.marketplaceOrderId,
        orderCreationDate: order.orderCreationDate,
        status: order.status,
        totalPrice: order.totalPrice,
      };
    });
    // console.log("partsterrOrders", partsterrOrders);
    // Return the orders as a JSON string
    return new NextResponse(JSON.stringify(partsterrOrders), {
      headers: { "Content-Type": "application/json" },
    });
  } catch (error) {
    console.log(error);
    return new NextResponse(
      JSON.stringify({ message: "Hello World", status: "error" })
    );
  }
}
