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
import type { Account } from "@/types";

interface DeleteAccountDialogProps {
  open: boolean;
  onClose: () => void;
  onConfirm: () => void;
  account: Account | null;
  isLoading: boolean;
}

export function DeleteAccountDialog({
  open,
  onClose,
  onConfirm,
  account,
  isLoading,
}: DeleteAccountDialogProps) {
  if (!account) return null;

  return (
    <AlertDialog open={open} onOpenChange={onClose}>
      <AlertDialogContent>
        <AlertDialogHeader>
          <AlertDialogTitle>Hesabı Sil</AlertDialogTitle>
          <AlertDialogDescription>
            <span className="font-semibold">{account.name}</span> hesabını
            silmek istediğinizden emin misiniz?
            <br />
            <br />
            Bu işlem geri alınamaz ve hesaba ait tüm işlemler silinecektir.
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
