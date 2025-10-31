import { format } from "date-fns";
import { tr } from "date-fns/locale";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import { Badge } from "@/components/ui/badge";
import type { RecentTransaction } from "@/types";

interface RecentTransactionsTableProps {
  transactions: RecentTransaction[];
}

export function RecentTransactionsTable({
  transactions,
}: RecentTransactionsTableProps) {
  if (transactions.length === 0) {
    return (
      <div className="text-center py-12 text-muted-foreground">
        <p>Henüz işlem bulunmuyor</p>
      </div>
    );
  }

  return (
    <Table>
      <TableHeader>
        <TableRow>
          <TableHead>Tarih</TableHead>
          <TableHead>Açıklama</TableHead>
          <TableHead>Kategori</TableHead>
          <TableHead className="text-right">Tutar</TableHead>
        </TableRow>
      </TableHeader>
      <TableBody>
        {transactions.map((transaction) => (
          <TableRow key={transaction.id}>
            <TableCell className="text-muted-foreground">
              {format(new Date(transaction.transactionDate), "dd MMM", {
                locale: tr,
              })}
            </TableCell>
            <TableCell className="font-medium">
              {transaction.description || "İşlem"}
            </TableCell>
            <TableCell>
              <Badge variant="outline">{transaction.categoryName}</Badge>
            </TableCell>
            <TableCell className="text-right">
              <span
                className={
                  transaction.type === "Income"
                    ? "text-green-600 font-semibold"
                    : "text-red-600 font-semibold"
                }
              >
                {transaction.type === "Income" ? "+" : "-"}
                {transaction.amount.toLocaleString("tr-TR")} ₺
              </span>
            </TableCell>
          </TableRow>
        ))}
      </TableBody>
    </Table>
  );
}
