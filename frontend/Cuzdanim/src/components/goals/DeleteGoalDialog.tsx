import {
  AlertDialog,
  AlertDialogAction,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
} from "@/components/ui/alert-dialog";
import type { Goal } from "@/types";
import { formatCurrency } from "@/lib/utils";

interface DeleteGoalDialogProps {
  open: boolean;
  onClose: () => void;
  onConfirm: () => void;
  goal: Goal | null;
  isLoading: boolean;
}

export function DeleteGoalDialog({
  open,
  onClose,
  onConfirm,
  goal,
  isLoading,
}: DeleteGoalDialogProps) {
  if (!goal) return null;

  const formatDate = (dateStr: string) => {
    const date = new Date(dateStr);
    return date.toLocaleDateString("tr-TR", {
      day: "2-digit",
      month: "long",
      year: "numeric",
    });
  };

  return (
    <AlertDialog open={open} onOpenChange={onClose}>
      <AlertDialogContent>
        <AlertDialogHeader>
          <AlertDialogTitle>Hedefi Sil</AlertDialogTitle>
          <AlertDialogDescription>
            <span className="font-semibold">{goal.name}</span> hedefini silmek
            istediğinizden emin misiniz?
            <br />
            <br />
            <div className="space-y-1 text-sm">
              <div className="flex justify-between">
                <span>Mevcut:</span>
                <span className="font-semibold">
                  {formatCurrency(goal.currentAmount, goal.currency)}
                </span>
              </div>
              <div className="flex justify-between">
                <span>Hedef:</span>
                <span className="font-semibold">
                  {formatCurrency(goal.targetAmount, goal.currency)}
                </span>
              </div>
              <div className="flex justify-between">
                <span>İlerleme:</span>
                <span className="font-semibold">
                  %{goal.progressPercentage.toFixed(0)}
                </span>
              </div>
              {goal.targetDate && (
                <div className="flex justify-between">
                  <span>Hedef Tarih:</span>
                  <span>{formatDate(goal.targetDate)}</span>
                </div>
              )}
            </div>
            <br />
            Bu işlem geri alınamaz.
          </AlertDialogDescription>
        </AlertDialogHeader>
        <AlertDialogFooter>
          <AlertDialogCancel disabled={isLoading}>İptal</AlertDialogCancel>
          <AlertDialogAction
            onClick={onConfirm}
            disabled={isLoading}
            className="bg-destructive hover:bg-destructive/90"
          >
            {isLoading ? "Siliniyor..." : "Sil"}
          </AlertDialogAction>
        </AlertDialogFooter>
      </AlertDialogContent>
    </AlertDialog>
  );
}
