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
import type { Transaction } from "@/types";
import { formatCurrency } from "@/lib/utils";

interface DeleteTransactionDialogProps {
  open: boolean;
  onClose: () => void;
  onConfirm: () => void;
  transaction: Transaction | null;
  isLoading: boolean;
}

export function DeleteTransactionDialog({
  open,
  onClose,
  onConfirm,
  transaction,
  isLoading,
}: DeleteTransactionDialogProps) {
  if (!transaction) return null;

  return (
    <AlertDialog open={open} onOpenChange={onClose}>
      <AlertDialogContent>
        <AlertDialogHeader>
          <AlertDialogTitle>İşlemi Sil</AlertDialogTitle>
          <AlertDialogDescription>
            <span className="font-semibold">
              {transaction.description || "İşlem"}
            </span>{" "}
            kaydını silmek istediğinizden emin misiniz?
            <br />
            <br />
            <span className="text-base font-semibold">
              {formatCurrency(transaction.amount, transaction.currency)}
            </span>
            <br />
            <span className="text-sm">
              {transaction.accountName} - {transaction.categoryName}
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
