import { Wallet, TrendingUp, TrendingDown, DollarSign } from "lucide-react";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { Skeleton } from "@/components/ui/skeleton";
import { useDashboard } from "@/hooks/useDashboard";
import { StatCard } from "@/components/dashboard/StatCard";
import { RecentTransactionsTable } from "@/components/dashboard/RecentTransactionsTable";
import { BudgetAlerts } from "@/components/dashboard/BudgetAlerts";
import { formatCurrency } from "@/lib/utils";

export default function DashboardPage() {
  const { data: dashboard, isLoading, error } = useDashboard();

  if (isLoading) {
    return (
      <div className="space-y-6">
        <div>
          <h1 className="text-3xl font-bold">Dashboard</h1>
          <p className="text-muted-foreground">Finansal durumunuzun özeti</p>
        </div>

        {/* Loading Skeletons */}
        <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
          {[1, 2, 3, 4].map((i) => (
            <Card key={i}>
              <CardContent className="p-6">
                <Skeleton className="h-4 w-24 mb-2" />
                <Skeleton className="h-8 w-32" />
              </CardContent>
            </Card>
          ))}
        </div>

        <div className="grid gap-6 md:grid-cols-2">
          <Card>
            <CardHeader>
              <Skeleton className="h-6 w-32" />
            </CardHeader>
            <CardContent>
              <Skeleton className="h-48 w-full" />
            </CardContent>
          </Card>
          <Card>
            <CardHeader>
              <Skeleton className="h-6 w-32" />
            </CardHeader>
            <CardContent>
              <Skeleton className="h-48 w-full" />
            </CardContent>
          </Card>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="space-y-6">
        <div>
          <h1 className="text-3xl font-bold">Dashboard</h1>
        </div>
        <Card>
          <CardContent className="p-6">
            <p className="text-destructive">
              Dashboard verileri yüklenirken bir hata oluştu: {error.message}
            </p>
          </CardContent>
        </Card>
      </div>
    );
  }

  if (!dashboard) {
    return null;
  }

  // Trend hesaplama
  const incomeTrend =
    dashboard.incomeChangePercentage > 0
      ? "up"
      : dashboard.incomeChangePercentage < 0
      ? "down"
      : "neutral";

  const expenseTrend =
    dashboard.expenseChangePercentage > 0
      ? "up"
      : dashboard.expenseChangePercentage < 0
      ? "down"
      : "neutral";

  const netTrend =
    dashboard.currentMonthNet > 0
      ? "up"
      : dashboard.currentMonthNet < 0
      ? "down"
      : "neutral";

  return (
    <div className="space-y-6">
      {/* Header */}
      <div>
        <h1 className="text-3xl font-bold">Dashboard</h1>
        <p className="text-muted-foreground">Finansal durumunuzun özeti</p>
      </div>

      {/* Stat Cards */}
      <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
        <StatCard
          title="Toplam Bakiye"
          value={formatCurrency(dashboard.totalBalance, dashboard.currency)}
          icon={Wallet}
        />
        <StatCard
          title="Bu Ay Gelir"
          value={formatCurrency(
            dashboard.currentMonthIncome,
            dashboard.currency
          )}
          change={dashboard.incomeChangePercentage}
          icon={TrendingUp}
          trend={incomeTrend}
        />
        <StatCard
          title="Bu Ay Gider"
          value={formatCurrency(
            dashboard.currentMonthExpense,
            dashboard.currency
          )}
          change={dashboard.expenseChangePercentage}
          icon={TrendingDown}
          trend={expenseTrend}
        />
        <StatCard
          title="Net Durum"
          value={formatCurrency(dashboard.currentMonthNet, dashboard.currency)}
          icon={DollarSign}
          trend={netTrend}
        />
      </div>

      {/* Bütçe Uyarıları */}
      {dashboard.budgetAlerts.length > 0 && (
        <div>
          <h2 className="text-xl font-semibold mb-4">Bütçe Uyarıları</h2>
          <BudgetAlerts alerts={dashboard.budgetAlerts} />
        </div>
      )}

      {/* İçerik Grid */}
      <div className="grid gap-6 md:grid-cols-2">
        {/* Son İşlemler */}
        <Card className="col-span-2">
          <CardHeader>
            <CardTitle>Son İşlemler</CardTitle>
            <CardDescription>En son gerçekleştirilen işlemler</CardDescription>
          </CardHeader>
          <CardContent>
            <RecentTransactionsTable
              transactions={dashboard.recentTransactions}
            />
          </CardContent>
        </Card>

        {/* Hesap Özeti */}
        <Card>
          <CardHeader>
            <CardTitle>Hesap Özeti</CardTitle>
          </CardHeader>
          <CardContent className="space-y-4">
            <div className="flex justify-between items-center">
              <span className="text-muted-foreground">Toplam Hesap</span>
              <span className="font-semibold">{dashboard.totalAccounts}</span>
            </div>
            <div className="flex justify-between items-center">
              <span className="text-muted-foreground">Aktif Hesap</span>
              <span className="font-semibold">{dashboard.activeAccounts}</span>
            </div>
          </CardContent>
        </Card>

        {/* Hedef Özeti */}
        <Card>
          <CardHeader>
            <CardTitle>Hedef Özeti</CardTitle>
          </CardHeader>
          <CardContent className="space-y-4">
            <div className="flex justify-between items-center">
              <span className="text-muted-foreground">Toplam Hedef</span>
              <span className="font-semibold">{dashboard.totalGoals}</span>
            </div>
            <div className="flex justify-between items-center">
              <span className="text-muted-foreground">Aktif Hedef</span>
              <span className="font-semibold">{dashboard.activeGoals}</span>
            </div>
            <div className="flex justify-between items-center">
              <span className="text-muted-foreground">Bu Ay Tamamlanan</span>
              <span className="font-semibold text-green-600">
                {dashboard.completedGoalsThisMonth}
              </span>
            </div>
          </CardContent>
        </Card>
      </div>
    </div>
  );
}
