import { AlertCircle, AlertTriangle } from "lucide-react";
import { Alert, AlertDescription, AlertTitle } from "@/components/ui/alert";
import { Progress } from "@/components/ui/progress";
import type { BudgetAlert } from "@/types";

interface BudgetAlertsProps {
  alerts: BudgetAlert[];
}

export function BudgetAlerts({ alerts }: BudgetAlertsProps) {
  if (alerts.length === 0) {
    return null; // Uyarı yoksa gösterme
  }

  return (
    <div className="space-y-4">
      {alerts.map((alert) => (
        <Alert
          key={alert.budgetId}
          variant={alert.alertLevel === "Danger" ? "destructive" : "default"}
          className={
            alert.alertLevel === "Warning"
              ? "border-orange-500 bg-orange-50 text-orange-900"
              : ""
          }
        >
          {alert.alertLevel === "Danger" ? (
            <AlertCircle className="h-4 w-4" />
          ) : (
            <AlertTriangle className="h-4 w-4" />
          )}
          <AlertTitle>
            {alert.categoryName} - {alert.budgetName}
          </AlertTitle>
          <AlertDescription className="mt-2 space-y-2">
            <p className="text-sm">
              Bütçenizin{" "}
              <span className="font-bold">
                %{alert.spentPercentage.toFixed(0)}
              </span>
              'sini kullandınız
            </p>
            <Progress value={alert.spentPercentage} className="h-2" />
            <p className="text-xs">
              {alert.spentAmount.toLocaleString("tr-TR")} ₺ /{" "}
              {alert.budgetAmount.toLocaleString("tr-TR")} ₺
            </p>
          </AlertDescription>
        </Alert>
      ))}
    </div>
  );
}
