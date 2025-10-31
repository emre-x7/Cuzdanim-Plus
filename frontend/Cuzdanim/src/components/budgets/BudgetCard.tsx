import { Pencil, Trash2, AlertTriangle, CheckCircle2 } from "lucide-react";
import { Card, CardContent } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import { Progress } from "@/components/ui/progress";
import { formatCurrency } from "@/lib/utils";
import type { Budget } from "@/types";

interface BudgetCardProps {
  budget: Budget;
  onEdit: (budget: Budget) => void;
  onDelete: (budget: Budget) => void;
}

export function BudgetCard({ budget, onEdit, onDelete }: BudgetCardProps) {
  // Status renkleri
  const statusConfig = {
    Normal: {
      badge: "bg-green-100 text-green-800 border-green-200",
      icon: CheckCircle2,
      iconColor: "text-green-600",
      progressColor: "bg-green-500",
      label: "Normal",
    },
    Warning: {
      badge: "bg-yellow-100 text-yellow-800 border-yellow-200",
      icon: AlertTriangle,
      iconColor: "text-yellow-600",
      progressColor: "bg-yellow-500",
      label: "Uyarı",
    },
    Exceeded: {
      badge: "bg-red-100 text-red-800 border-red-200",
      icon: AlertTriangle,
      iconColor: "text-red-600",
      progressColor: "bg-red-500",
      label: "Aşıldı",
    },
  } as const;

  // Backend göndermiyorsa default değerler
  const spent = budget.spent ?? 0;
  const remaining = budget.remaining ?? budget.amount;
  const percentageUsed =
    budget.percentageUsed ??
    (budget.amount > 0 ? (spent / budget.amount) * 100 : 0);

  // Status hesapla (backend göndermiyorsa)
  let status: "Normal" | "Warning" | "Exceeded" = budget.status || "Normal";

  if (!budget.status) {
    // Frontend'de hesapla
    if (percentageUsed >= 100) {
      status = "Exceeded";
    } else if (percentageUsed >= budget.alertThresholdPercentage) {
      status = "Warning";
    } else {
      status = "Normal";
    }
  }

  const config = statusConfig[status] || statusConfig.Normal;
  const StatusIcon = config.icon;

  // Tarih formatı
  const formatDate = (dateStr: string) => {
    if (!dateStr) return "";
    const date = new Date(dateStr);
    return date.toLocaleDateString("tr-TR", { day: "2-digit", month: "short" });
  };

  return (
    <Card className="group hover:shadow-lg transition-shadow">
      <CardContent className="p-6">
        <div className="flex items-start justify-between mb-4">
          {/* Kategori Bilgisi */}
          <div className="flex items-center gap-3">
            {budget.categoryIcon && (
              <div
                className="w-12 h-12 rounded-lg flex items-center justify-center text-2xl"
                style={{
                  backgroundColor: `${budget.categoryColor || "#ccc"}20`,
                }}
              >
                <span style={{ color: budget.categoryColor || "#666" }}>
                  {budget.categoryIcon}
                </span>
              </div>
            )}
            <div>
              <h3 className="font-semibold text-lg">{budget.name}</h3>
              <p className="text-sm text-muted-foreground">
                {budget.categoryName}
              </p>
              {budget.periodStartDate && budget.periodEndDate && (
                <p className="text-xs text-muted-foreground">
                  {formatDate(budget.periodStartDate)} -{" "}
                  {formatDate(budget.periodEndDate)}
                </p>
              )}
            </div>
          </div>

          {/* Status Badge */}
          <Badge variant="outline" className={config.badge}>
            <StatusIcon className={`w-3 h-3 mr-1 ${config.iconColor}`} />
            {config.label}
          </Badge>
        </div>

        {/* Tutar Bilgisi */}
        <div className="mb-4">
          <div className="flex items-baseline justify-between mb-2">
            <div>
              <span className="text-2xl font-bold">
                {formatCurrency(spent, budget.currency)}
              </span>
              <span className="text-muted-foreground mx-2">/</span>
              <span className="text-lg text-muted-foreground">
                {formatCurrency(budget.amount, budget.currency)}
              </span>
            </div>
            <span className="text-sm font-medium">
              {percentageUsed.toFixed(0)}%
            </span>
          </div>

          {/* Progress Bar */}
          <Progress
            value={Math.min(percentageUsed, 100)}
            className="h-2"
            indicatorClassName={config.progressColor}
          />
        </div>

        {/* Kalan Tutar */}
        <div className="flex items-center justify-between text-sm mb-4">
          <span className="text-muted-foreground">Kalan</span>
          <span
            className={
              remaining < 0 ? "text-red-600 font-semibold" : "font-semibold"
            }
          >
            {formatCurrency(Math.abs(remaining), budget.currency)}
            {remaining < 0 && " fazla harcandı"}
          </span>
        </div>

        {/* Uyarı Eşiği Bilgisi */}
        {budget.alertThresholdPercentage > 0 && (
          <div className="text-xs text-muted-foreground mb-4">
            Uyarı eşiği: %{budget.alertThresholdPercentage}
          </div>
        )}

        {/* Action Buttons */}
        <div className="flex gap-2 opacity-0 group-hover:opacity-100 transition-opacity">
          <Button
            variant="outline"
            size="sm"
            className="flex-1"
            onClick={() => onEdit(budget)}
          >
            <Pencil className="w-3 h-3 mr-1" />
            Düzenle
          </Button>
          <Button
            variant="outline"
            size="sm"
            className="text-destructive hover:bg-destructive hover:text-destructive-foreground"
            onClick={() => onDelete(budget)}
          >
            <Trash2 className="w-3 h-3" />
          </Button>
        </div>
      </CardContent>
    </Card>
  );
}
