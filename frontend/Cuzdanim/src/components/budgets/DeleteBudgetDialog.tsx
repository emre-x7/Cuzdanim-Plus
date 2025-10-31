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
import type { Budget } from "@/types";
import { formatCurrency } from "@/lib/utils";

interface DeleteBudgetDialogProps {
  open: boolean;
  onClose: () => void;
  onConfirm: () => void;
  budget: Budget | null;
  isLoading: boolean;
}

export function DeleteBudgetDialog({
  open,
  onClose,
  onConfirm,
  budget,
  isLoading,
}: DeleteBudgetDialogProps) {
  if (!budget) return null;

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
          <AlertDialogTitle>Bütçeyi Sil</AlertDialogTitle>
          <AlertDialogDescription>
            <span className="font-semibold">{budget.name}</span> bütçesini
            silmek istediğinizden emin misiniz?
            <br />
            <br />
            <span className="text-base font-semibold">
              {formatCurrency(budget.amount, budget.currency)}
            </span>
            <br />
            <span className="text-sm">{budget.categoryName}</span>
            <br />
            <span className="text-xs text-muted-foreground">
              {formatDate(budget.periodStartDate)} -{" "}
              {formatDate(budget.periodEndDate)}
            </span>
            <br />
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
