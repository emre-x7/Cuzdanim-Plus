import { Card, CardContent } from "@/components/ui/card";
import { type LucideIcon } from "lucide-react";
import { cn } from "@/lib/utils";

interface StatCardProps {
  title: string;
  value: string;
  change?: number; // Değişim yüzdesi
  icon: LucideIcon;
  trend?: "up" | "down" | "neutral";
}

export function StatCard({
  title,
  value,
  change,
  icon: Icon,
  trend = "neutral",
}: StatCardProps) {
  const trendColor = {
    up: "text-green-600",
    down: "text-red-600",
    neutral: "text-muted-foreground",
  };

  return (
    <Card>
      <CardContent className="p-6">
        <div className="flex items-center justify-between">
          <div className="flex-1">
            <p className="text-sm font-medium text-muted-foreground">{title}</p>
            <h3 className="text-2xl font-bold mt-2">{value}</h3>
            {change !== undefined && (
              <p
                className={cn(
                  "text-sm mt-2 flex items-center gap-1",
                  trendColor[trend]
                )}
              >
                <span>
                  {trend === "up" ? "↑" : trend === "down" ? "↓" : "→"}
                </span>
                <span>{Math.abs(change).toFixed(1)}%</span>
                <span className="text-muted-foreground">geçen aya göre</span>
              </p>
            )}
          </div>
          <div className="w-12 h-12 bg-primary/10 rounded-lg flex items-center justify-center">
            <Icon className="w-6 h-6 text-primary" />
          </div>
        </div>
      </CardContent>
    </Card>
  );
}
