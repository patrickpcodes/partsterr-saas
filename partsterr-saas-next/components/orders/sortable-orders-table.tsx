"use client";

import { useState } from "react";
import {
  ArrowDownIcon,
  ArrowUpIcon,
  CalendarDays,
  CarIcon as CaretSortIcon,
} from "lucide-react";
import Image from "next/image";

import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import { PartsterrOrder } from "@/lib/types/Order";

type SortConfig = {
  key: keyof PartsterrOrder;
  direction: "asc" | "desc";
} | null;

type SortableOrdersTableProps = {
  orders: PartsterrOrder[];
};

export default function SortableOrdersTable({
  orders,
}: SortableOrdersTableProps) {
  const [sortConfig, setSortConfig] = useState<SortConfig>(null);

  const sortedOrders = [...orders].sort((a, b) => {
    if (!sortConfig) return 0;

    const { key, direction } = sortConfig;
    if (a[key] < b[key]) return direction === "asc" ? -1 : 1;
    if (a[key] > b[key]) return direction === "asc" ? 1 : -1;
    return 0;
  });

  const requestSort = (key: keyof PartsterrOrder) => {
    let direction: "asc" | "desc" = "asc";
    if (
      sortConfig &&
      sortConfig.key === key &&
      sortConfig.direction === "asc"
    ) {
      direction = "desc";
    }
    setSortConfig({ key, direction });
  };

  const getSortIcon = (columnKey: keyof PartsterrOrder) => {
    if (!sortConfig || sortConfig.key !== columnKey) {
      return <CaretSortIcon className="ml-2 h-4 w-4" />;
    }
    return sortConfig.direction === "asc" ? (
      <ArrowUpIcon className="ml-2 h-4 w-4" />
    ) : (
      <ArrowDownIcon className="ml-2 h-4 w-4" />
    );
  };

  const getStatusColor = (status: string) => {
    switch (status.toLowerCase()) {
      case "processing":
        return "bg-yellow-100 text-yellow-800 dark:bg-yellow-900 dark:text-yellow-200";
      case "shipped":
        return "bg-blue-100 text-blue-800 dark:bg-blue-900 dark:text-blue-200";
      case "delivered":
        return "bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-200";
      default:
        return "bg-gray-100 text-gray-800 dark:bg-gray-900 dark:text-gray-200";
    }
  };

  return (
    <div className="rounded-lg border bg-card">
      <Table>
        <TableHeader>
          <TableRow className="hover:bg-muted/50">
            <TableHead className="w-[100px]">Marketplace</TableHead>
            <TableHead>
              <Button
                variant="ghost"
                className="p-0 font-semibold"
                onClick={() => requestSort("description")}
              >
                Product Name
                {getSortIcon("description")}
              </Button>
            </TableHead>
            <TableHead>
              <Button
                variant="ghost"
                className="p-0 font-semibold"
                onClick={() => requestSort("marketplaceOrderId")}
              >
                Order Number
                {getSortIcon("marketplaceOrderId")}
              </Button>
            </TableHead>
            <TableHead>
              <Button
                variant="ghost"
                className="p-0 font-semibold"
                onClick={() => requestSort("orderCreationDate")}
              >
                Date
                {getSortIcon("orderCreationDate")}
              </Button>
            </TableHead>
            <TableHead>
              <Button
                variant="ghost"
                className="p-0 font-semibold"
                onClick={() => requestSort("status")}
              >
                Status
                {getSortIcon("status")}
              </Button>
            </TableHead>
            <TableHead className="text-right">
              <Button
                variant="ghost"
                className="p-0 font-semibold"
                onClick={() => requestSort("totalPrice")}
              >
                Total
                {getSortIcon("totalPrice")}
              </Button>
            </TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          {sortedOrders.map((order) => (
            <TableRow
              key={order.marketplaceOrderId}
              className="hover:bg-muted/50"
            >
              <TableCell>
                <div className="relative h-10 w-10">
                  <Image
                    src={`/${order.marketplace}-logo.png`}
                    alt={`${order.marketplace} logo`}
                    fill
                    className="object-contain"
                  />
                </div>
              </TableCell>

              <TableCell className="font-medium">
                {order.description.length > 40
                  ? `${order.description.substring(0, 40)}...`
                  : order.description}
              </TableCell>
              <TableCell>
                <span className="font-mono text-sm">
                  {order.marketplaceOrderId}
                </span>
              </TableCell>
              <TableCell>
                <div className="flex items-center gap-2">
                  <CalendarDays className="h-4 w-4 text-muted-foreground" />
                  <span>
                    {new Date(order.orderCreationDate).toLocaleDateString()}
                  </span>
                </div>
              </TableCell>
              <TableCell>
                <Badge
                  variant="secondary"
                  className={getStatusColor(order.status)}
                >
                  {order.status}
                </Badge>
              </TableCell>
              <TableCell className="text-right">${order.totalPrice}</TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </div>
  );
}
