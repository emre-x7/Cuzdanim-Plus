import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { Progress } from "@/components/ui/progress";
import type { CategoryReport } from "@/types";
import { formatCurrency } from "@/lib/utils";

interface CategoryDetailsTableProps {
  data: CategoryReport[];
  title: string;
  currency: string;
  type: "income" | "expense";
}

export function CategoryDetailsTable({
  data,
  title,
  currency,
  type,
}: CategoryDetailsTableProps) {
  if (!data || data.length === 0) {
    return (
      <Card>
        <CardHeader>
          <CardTitle>{title}</CardTitle>
        </CardHeader>
        <CardContent>
          <p className="text-center text-muted-foreground py-8">
            Veri bulunamadı
          </p>
        </CardContent>
      </Card>
    );
  }

  const totalAmount = data.reduce((sum, item) => sum + item.totalAmount, 0);

  return (
    <Card>
      <CardHeader>
        <CardTitle>{title}</CardTitle>
      </CardHeader>
      <CardContent>
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead>Kategori</TableHead>
              <TableHead className="text-right">İşlem Sayısı</TableHead>
              <TableHead className="text-right">Tutar</TableHead>
              <TableHead className="text-right">Oran</TableHead>
              <TableHead className="w-[200px]">Dağılım</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {data.map((item) => (
              <TableRow key={item.categoryId}>
                <TableCell>
                  <div className="flex items-center gap-2">
                    {item.categoryIcon && (
                      <div
                        className="w-8 h-8 rounded-lg flex items-center justify-center"
                        style={{
                          backgroundColor: `${item.categoryColor || "#ccc"}20`,
                        }}
                      >
                        <span style={{ color: item.categoryColor || "#666" }}>
                          {item.categoryIcon}
                        </span>
                      </div>
                    )}
                    <span className="font-medium">{item.categoryName}</span>
                  </div>
                </TableCell>
                <TableCell className="text-right">
                  <Badge variant="outline">{item.transactionCount}</Badge>
                </TableCell>
                <TableCell className="text-right font-semibold">
                  {formatCurrency(item.totalAmount, currency)}
                </TableCell>
                <TableCell className="text-right">
                  <span className="text-sm font-medium">
                    %{item.percentage.toFixed(1)}
                  </span>
                </TableCell>
                <TableCell>
                  <Progress
                    value={item.percentage}
                    className="h-2"
                    indicatorClassName={
                      type === "income" ? "bg-green-500" : "bg-red-500"
                    }
                  />
                </TableCell>
              </TableRow>
            ))}
            {/* Toplam Row */}
            <TableRow className="font-bold bg-muted/50">
              <TableCell>Toplam</TableCell>
              <TableCell className="text-right">
                {data.reduce((sum, item) => sum + item.transactionCount, 0)}
              </TableCell>
              <TableCell className="text-right">
                {formatCurrency(totalAmount, currency)}
              </TableCell>
              <TableCell className="text-right">%100</TableCell>
              <TableCell></TableCell>
            </TableRow>
          </TableBody>
        </Table>
      </CardContent>
    </Card>
  );
}
