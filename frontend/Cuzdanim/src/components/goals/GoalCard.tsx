import {
  Pencil,
  Trash2,
  Plus,
  Calendar,
  Target,
  TrendingUp,
} from "lucide-react";
import { Card, CardContent } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import { Progress } from "@/components/ui/progress";
import { formatCurrency } from "@/lib/utils";
import type { Goal } from "@/types";

interface GoalCardProps {
  goal: Goal;
  onEdit: (goal: Goal) => void;
  onDelete: (goal: Goal) => void;
  onAddContribution: (goal: Goal) => void;
}

export function GoalCard({
  goal,
  onEdit,
  onDelete,
  onAddContribution,
}: GoalCardProps) {
  // Status renkleri
  const statusConfig = {
    Active: {
      badge: "bg-blue-100 text-blue-800 border-blue-200",
      progressColor: "bg-blue-500",
      label: "Aktif",
    },
    Completed: {
      badge: "bg-green-100 text-green-800 border-green-200",
      progressColor: "bg-green-500",
      label: "Tamamlandı",
    },
    Expired: {
      badge: "bg-red-100 text-red-800 border-red-200",
      progressColor: "bg-red-500",
      label: "Süresi Doldu",
    },
  };

  const config =
    statusConfig[goal.status as keyof typeof statusConfig] ||
    statusConfig.Active;

  // Gün kaldı mesajı
  const getDaysRemainingText = () => {
    if (goal.status === "Completed") return "Hedef tamamlandı! 🎉";
    if (goal.status === "Expired") return "Süre doldu";
    if (goal.daysRemaining < 0) return "Süre geçti";
    if (goal.daysRemaining === 0) return "Bugün son gün!";
    if (goal.daysRemaining === 1) return "1 gün kaldı";
    return `${goal.daysRemaining} gün kaldı`;
  };

  const daysRemainingColor =
    goal.daysRemaining <= 7 ? "text-red-600" : "text-muted-foreground";

  return (
    <Card className="group hover:shadow-lg transition-shadow">
      <CardContent className="p-6">
        <div className="flex items-start justify-between mb-4">
          {/* Hedef Bilgisi */}
          <div className="flex items-center gap-3">
            {goal.icon && (
              <div className="w-12 h-12 rounded-lg bg-primary/10 flex items-center justify-center text-2xl">
                {goal.icon}
              </div>
            )}
            {!goal.icon && (
              <div className="w-12 h-12 rounded-lg bg-primary/10 flex items-center justify-center">
                <Target className="w-6 h-6 text-primary" />
              </div>
            )}
            <div>
              <h3 className="font-semibold text-lg">{goal.name}</h3>
              {goal.description && (
                <p className="text-sm text-muted-foreground line-clamp-1">
                  {goal.description}
                </p>
              )}
            </div>
          </div>

          {/* Status Badge */}
          <Badge variant="outline" className={config.badge}>
            {config.label}
          </Badge>
        </div>

        {/* Tutar Bilgisi */}
        <div className="mb-4">
          <div className="flex items-baseline justify-between mb-2">
            <div>
              <span className="text-2xl font-bold">
                {formatCurrency(goal.currentAmount, goal.currency)}
              </span>
              <span className="text-muted-foreground mx-2">/</span>
              <span className="text-lg text-muted-foreground">
                {formatCurrency(goal.targetAmount, goal.currency)}
              </span>
            </div>
            <span className="text-sm font-medium">
              {goal.progressPercentage.toFixed(0)}%
            </span>
          </div>

          {/* Progress Bar */}
          <Progress
            value={Math.min(goal.progressPercentage, 100)}
            className="h-2"
            indicatorClassName={config.progressColor}
          />
        </div>

        {/* Detaylar */}
        <div className="space-y-2 mb-4">
          {/* Kalan Tutar */}
          <div className="flex items-center justify-between text-sm">
            <div className="flex items-center gap-2 text-muted-foreground">
              <TrendingUp className="w-4 h-4" />
              <span>Kalan</span>
            </div>
            <span className="font-semibold">
              {formatCurrency(goal.remainingAmount, goal.currency)}
            </span>
          </div>

          {/* Deadline */}
          <div className="flex items-center justify-between text-sm">
            <div className="flex items-center gap-2 text-muted-foreground">
              <Calendar className="w-4 h-4" />
              <span>Hedef Tarih</span>
            </div>
            <span className={daysRemainingColor}>{getDaysRemainingText()}</span>
          </div>
        </div>

        {/* Action Buttons */}
        <div className="flex gap-2">
          {goal.status === "Active" && (
            <Button
              variant="default"
              size="sm"
              className="flex-1"
              onClick={() => onAddContribution(goal)}
            >
              <Plus className="w-3 h-3 mr-1" />
              Katkı Ekle
            </Button>
          )}

          <div className="flex gap-2 opacity-0 group-hover:opacity-100 transition-opacity">
            <Button variant="outline" size="sm" onClick={() => onEdit(goal)}>
              <Pencil className="w-3 h-3" />
            </Button>
            <Button
              variant="outline"
              size="sm"
              className="text-destructive hover:bg-destructive hover:text-destructive-foreground"
              onClick={() => onDelete(goal)}
            >
              <Trash2 className="w-3 h-3" />
            </Button>
          </div>
        </div>
      </CardContent>
    </Card>
  );
}
