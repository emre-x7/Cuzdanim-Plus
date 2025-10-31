import { format } from "date-fns";
import { tr } from "date-fns/locale";
import { Pencil, Trash2, TrendingUp, TrendingDown } from "lucide-react";
import { Card, CardContent } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import { formatCurrency } from "@/lib/utils";
import type { Transaction } from "@/types";

interface TransactionCardProps {
  transaction: Transaction;
  onEdit: (transaction: Transaction) => void;
  onDelete: (transaction: Transaction) => void;
}

export function TransactionCard({
  transaction,
  onEdit,
  onDelete,
}: TransactionCardProps) {
  const isIncome = transaction.type === "Income";
  const Icon = isIncome ? TrendingUp : TrendingDown;
  const amountColor = isIncome ? "text-green-600" : "text-red-600";
  const iconBg = isIncome ? "bg-green-100" : "bg-red-100";
  const iconColor = isIncome ? "text-green-600" : "text-red-600";

  return (
    <Card className="group hover:shadow-lg transition-shadow">
      <CardContent className="p-4">
        <div className="flex items-center gap-4">
          {/* Icon + Date */}
          <div className="flex flex-col items-center gap-1">
            <div
              className={`w-12 h-12 rounded-lg flex items-center justify-center ${iconBg}`}
            >
              <Icon className={`w-6 h-6 ${iconColor}`} />
            </div>
            <span className="text-xs text-muted-foreground">
              {format(new Date(transaction.transactionDate), "dd MMM", {
                locale: tr,
              })}
            </span>
          </div>

          {/* Content */}
          <div className="flex-1 min-w-0">
            <div className="flex items-start justify-between gap-2">
              <div className="flex-1 min-w-0">
                <h4 className="font-semibold truncate">
                  {transaction.description || "İşlem"}
                </h4>
                <div className="flex items-center gap-2 mt-1">
                  <Badge variant="outline" className="text-xs">
                    {transaction.categoryName}
                  </Badge>
                  <span className="text-xs text-muted-foreground">
                    {transaction.accountName}
                  </span>
                </div>
              </div>

              {/* Amount */}
              <div className="text-right">
                <p className={`text-lg font-bold ${amountColor}`}>
                  {isIncome ? "+" : "-"}
                  {formatCurrency(transaction.amount, transaction.currency)}
                </p>
              </div>
            </div>

            {/* Notes */}
            {transaction.notes && (
              <p className="text-sm text-muted-foreground mt-2 line-clamp-2">
                {transaction.notes}
              </p>
            )}
          </div>

          {/* Actions (Hover'da görünür) */}
          <div className="flex flex-col gap-2 opacity-0 group-hover:opacity-100 transition-opacity">
            <Button
              variant="outline"
              size="icon"
              className="h-8 w-8"
              onClick={() => onEdit(transaction)}
            >
              <Pencil className="w-4 h-4" />
            </Button>
            <Button
              variant="outline"
              size="icon"
              className="h-8 w-8 text-destructive hover:bg-destructive hover:text-destructive-foreground"
              onClick={() => onDelete(transaction)}
            >
              <Trash2 className="w-4 h-4" />
            </Button>
          </div>
        </div>
      </CardContent>
    </Card>
  );
}
