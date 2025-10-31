import {
  BarChart,
  Bar,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  Legend,
  ResponsiveContainer,
} from "recharts";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import type { IncomeExpenseComparison } from "@/types";
import { formatCurrency } from "@/lib/utils";

interface IncomeExpenseChartProps {
  data: IncomeExpenseComparison;
  currency: string;
}

export function IncomeExpenseChart({
  data,
  currency,
}: IncomeExpenseChartProps) {
  const chartData = [
    {
      name: "Gelir vs Gider",
      Gelir: data.income,
      Gider: data.expense,
    },
  ];

  return (
    <Card>
      <CardHeader>
        <CardTitle>Gelir vs Gider</CardTitle>
      </CardHeader>
      <CardContent>
        <ResponsiveContainer width="100%" height={300}>
          <BarChart data={chartData}>
            <CartesianGrid strokeDasharray="3 3" />
            <XAxis dataKey="name" />
            <YAxis />
            <Tooltip
              formatter={(value: number) => formatCurrency(value, currency)}
            />
            <Legend />
            <Bar dataKey="Gelir" fill="#10b981" />
            <Bar dataKey="Gider" fill="#ef4444" />
          </BarChart>
        </ResponsiveContainer>
      </CardContent>
    </Card>
  );
}
