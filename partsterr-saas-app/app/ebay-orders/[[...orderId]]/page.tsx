// app/ebay-orders/[[...orderId]]/page.tsx
"use client";

import { useParams, useRouter } from "next/navigation";
import { useState, useEffect } from "react";
import {
  EbayOrder,
  ebaySettings,
  getAllEbayOrdersAfterDate,
  getEbayOrderById,
} from "@/lib/ebayApi";

// This component handles both the list and detail view.
export default function EbayOrdersPage() {
  // The catch-all parameter "orderId" will be an array if present.
  const params = useParams();
  const router = useRouter();
  const orderIdParam = params?.orderId; // might be undefined or an array
  const orderId = Array.isArray(orderIdParam) ? orderIdParam[0] : orderIdParam;

  const [orders, setOrders] = useState<EbayOrder[]>([]);
  const [selectedOrder, setSelectedOrder] = useState<EbayOrder | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    // If there's an orderId in the URL, fetch that single order.
    if (orderId && !selectedOrder) {
      (async () => {
        try {
          console.log("selected order before fetch", selectedOrder);
          const response = await fetch("/api/ebay-orders/" + orderId);
          const order = await response.json();
          setSelectedOrder(order);
        } catch (error) {
          console.error("Failed to fetch order by id:", error);
        } finally {
          setLoading(false);
        }
      })();
    } else {
      // No orderId in the URL, so fetch the list of orders.
      (async () => {
        try {
          const response = await fetch("/api/ebay-orders");
          const orders = await response.json();
          setOrders(orders);
        } catch (error) {
          console.error("Failed to load orders:", error);
        } finally {
          setLoading(false);
        }
      })();
    }
  }, [orderId]);

  if (loading) {
    return <p>Loading...</p>;
  }

  // Render the detail view if an orderId is present.
  if (orderId) {
    if (!selectedOrder) {
      return <p>Order not found.</p>;
    }

    return (
      <div style={{ padding: "1rem" }}>
        <button
          onClick={() => router.push("/ebay-orders")}
          style={{ marginBottom: "1rem" }}
        >
          ← Back to Orders
        </button>
        <h1>Order: {selectedOrder.orderId}</h1>
        <p>
          <strong>Subtotal:</strong>{" "}
          {selectedOrder.pricingSummary.priceSubtotal.value}{" "}
          {selectedOrder.pricingSummary.priceSubtotal.currency}
        </p>
        <p>
          <strong>Delivery Cost:</strong>{" "}
          {selectedOrder.pricingSummary.deliveryCost.value}{" "}
          {selectedOrder.pricingSummary.deliveryCost.currency}
        </p>
        <p>
          <strong>Total:</strong> {selectedOrder.pricingSummary.total.value}{" "}
          {selectedOrder.pricingSummary.total.currency}
        </p>
        {/* Render additional details as needed */}
      </div>
    );
  }

  // Otherwise, render the list view.
  return (
    <div className="">
      <h1>All eBay Orders</h1>
      <table>
        <thead>
          <tr>
            <th>Order ID</th>
            <th>Subtotal</th>
            <th>Total</th>
          </tr>
        </thead>
        <tbody>
          {orders.map((order) => (
            <tr
              key={order.orderId}
              style={{ cursor: "pointer" }}
              onClick={() => {
                setSelectedOrder(order);
                router.push(`/ebay-orders/${order.orderId}`);
              }}
            >
              <td>{order.orderId}</td>
              <td>{order.pricingSummary.priceSubtotal.value}</td>
              <td>{order.pricingSummary.total.value}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
