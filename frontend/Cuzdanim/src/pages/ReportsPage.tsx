import { useState } from "react";
import { TrendingUp, TrendingDown, DollarSign } from "lucide-react";
import { Card, CardContent } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Skeleton } from "@/components/ui/skeleton";
import { useReports } from "@/hooks/useReports";
import { IncomeExpenseChart } from "@/components/reports/IncomeExpenseChart";
import { CategoryPieChart } from "@/components/reports/CategoryPieChart";
import { MonthlyTrendChart } from "@/components/reports/MonthlyTrendChart";
import { CategoryDetailsTable } from "@/components/reports/CategoryDetailsTable";
import { formatCurrency } from "@/lib/utils";

export default function ReportsPage() {
  // Tarih filtreleme (son 30 gün default)
  const getDefaultDates = () => {
    const endDate = new Date();
    const startDate = new Date();
    startDate.setDate(startDate.getDate() - 30);

    return {
      startDate: startDate.toISOString().split("T")[0],
      endDate: endDate.toISOString().split("T")[0],
    };
  };

  const [dateRange, setDateRange] = useState(getDefaultDates());

  // API Query
  const { data: reportData, isLoading } = useReports({
    startDate: dateRange.startDate,
    endDate: dateRange.endDate,
  });

  const handleFilterChange = (
    field: "startDate" | "endDate",
    value: string
  ) => {
    setDateRange((prev) => ({ ...prev, [field]: value }));
  };

  const handleQuickFilter = (days: number) => {
    const endDate = new Date();
    const startDate = new Date();
    startDate.setDate(startDate.getDate() - days);

    setDateRange({
      startDate: startDate.toISOString().split("T")[0],
      endDate: endDate.toISOString().split("T")[0],
    });
  };

  if (isLoading) {
    return (
      <div className="space-y-6">
        <Skeleton className="h-10 w-48" />
        <div className="grid gap-6 md:grid-cols-3">
          {[1, 2, 3].map((i) => (
            <Skeleton key={i} className="h-32" />
          ))}
        </div>
        <Skeleton className="h-96" />
      </div>
    );
  }

  if (!reportData) {
    return (
      <div className="text-center py-12">
        <p className="text-muted-foreground">Rapor verisi yüklenemedi</p>
      </div>
    );
  }

  const {
    summary,
    incomeByCategory,
    expenseByCategory,
    monthlyTrend,
    comparison,
  } = reportData;

  const currency = summary.currency || "TRY";

  return (
    <div className="space-y-6">
      {/* Header */}
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold">Raporlar</h1>
          <p className="text-muted-foreground">Gelir ve gider analizleri</p>
        </div>
      </div>

      {/* Date Filter */}
      <Card>
        <CardContent className="p-6">
          <div className="flex flex-col md:flex-row gap-4">
            {/* Quick Filters */}
            <div className="flex gap-2 flex-wrap">
              <Button
                variant="outline"
                size="sm"
                onClick={() => handleQuickFilter(7)}
              >
                Son 7 Gün
              </Button>
              <Button
                variant="outline"
                size="sm"
                onClick={() => handleQuickFilter(30)}
              >
                Son 30 Gün
              </Button>
              <Button
                variant="outline"
                size="sm"
                onClick={() => handleQuickFilter(90)}
              >
                Son 90 Gün
              </Button>
              <Button
                variant="outline"
                size="sm"
                onClick={() => handleQuickFilter(365)}
              >
                Son 1 Yıl
              </Button>
            </div>

            {/* Custom Date Range */}
            <div className="flex gap-4 flex-1">
              <div className="flex-1">
                <Label htmlFor="startDate" className="text-xs">
                  Başlangıç
                </Label>
                <Input
                  id="startDate"
                  type="date"
                  value={dateRange.startDate}
                  onChange={(e) =>
                    handleFilterChange("startDate", e.target.value)
                  }
                />
              </div>
              <div className="flex-1">
                <Label htmlFor="endDate" className="text-xs">
                  Bitiş
                </Label>
                <Input
                  id="endDate"
                  type="date"
                  value={dateRange.endDate}
                  onChange={(e) =>
                    handleFilterChange("endDate", e.target.value)
                  }
                />
              </div>
            </div>
          </div>
        </CardContent>
      </Card>

      {/* Summary Cards */}
      <div className="grid gap-6 md:grid-cols-3">
        {/* Toplam Gelir */}
        <Card>
          <CardContent className="p-6">
            <div className="flex items-center justify-between">
              <div>
                <p className="text-sm text-muted-foreground">Toplam Gelir</p>
                <p className="text-2xl font-bold text-green-600">
                  {formatCurrency(summary.totalIncome, currency)}
                </p>
              </div>
              <div className="w-12 h-12 bg-green-100 rounded-full flex items-center justify-center">
                <TrendingUp className="w-6 h-6 text-green-600" />
              </div>
            </div>
          </CardContent>
        </Card>

        {/* Toplam Gider */}
        <Card>
          <CardContent className="p-6">
            <div className="flex items-center justify-between">
              <div>
                <p className="text-sm text-muted-foreground">Toplam Gider</p>
                <p className="text-2xl font-bold text-red-600">
                  {formatCurrency(summary.totalExpense, currency)}
                </p>
              </div>
              <div className="w-12 h-12 bg-red-100 rounded-full flex items-center justify-center">
                <TrendingDown className="w-6 h-6 text-red-600" />
              </div>
            </div>
          </CardContent>
        </Card>

        {/* Net */}
        <Card>
          <CardContent className="p-6">
            <div className="flex items-center justify-between">
              <div>
                <p className="text-sm text-muted-foreground">Net</p>
                <p
                  className={`text-2xl font-bold ${
                    summary.netAmount >= 0 ? "text-blue-600" : "text-red-600"
                  }`}
                >
                  {formatCurrency(summary.netAmount, currency)}
                </p>
              </div>
              <div className="w-12 h-12 bg-blue-100 rounded-full flex items-center justify-center">
                <DollarSign className="w-6 h-6 text-blue-600" />
              </div>
            </div>
          </CardContent>
        </Card>
      </div>

      {/* Income vs Expense Chart */}
      <IncomeExpenseChart data={comparison} currency={currency} />

      {/* Pie Charts */}
      <div className="grid gap-6 md:grid-cols-2">
        <CategoryPieChart
          data={incomeByCategory}
          title="Gelir Kategorileri"
          currency={currency}
        />
        <CategoryPieChart
          data={expenseByCategory}
          title="Gider Kategorileri"
          currency={currency}
        />
      </div>

      {/* Monthly Trend */}
      <MonthlyTrendChart data={monthlyTrend} currency={currency} />

      {/* Category Details Tables */}
      <div className="grid gap-6">
        <CategoryDetailsTable
          data={incomeByCategory}
          title="Gelir Kategorileri Detayı"
          currency={currency}
          type="income"
        />
        <CategoryDetailsTable
          data={expenseByCategory}
          title="Gider Kategorileri Detayı"
          currency={currency}
          type="expense"
        />
      </div>
    </div>
  );
}
