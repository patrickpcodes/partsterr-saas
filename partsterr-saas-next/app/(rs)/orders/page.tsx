"use client";

import SortableOrdersTable from "@/components/orders/sortable-orders-table";
import { PartsterrOrder } from "@/lib/types/Order";
import { useEffect, useState } from "react";

export default function OrderPage() {
  const [orders, setOrders] = useState<PartsterrOrder[]>([]);

  const fetchOrders = async () => {
    const response = await fetch("/api/orders", {
      method: "GET", // Ensure the method is GET
    });
    const data = await response.json();
    console.log("data", data);
    setOrders(data);
  };

  useEffect(() => {
    fetchOrders();
  }, []);

  return (
    <div className="m-2 p-4">
      {orders && orders.length > 0 && <SortableOrdersTable orders={orders} />}
    </div>
  );
}
