import { useState } from "react";
import { Plus } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Skeleton } from "@/components/ui/skeleton";
import {
  useAccounts,
  useCreateAccount,
  useUpdateAccount,
  useDeleteAccount,
} from "@/hooks/useAccounts";
import { AccountCard } from "@/components/accounts/AccountCard";
import { AccountFormModal } from "@/components/accounts/AccountFormModal";
import { DeleteAccountDialog } from "@/components/accounts/DeleteAccountDialog";
import type { Account } from "@/types";
import type { AccountFormData } from "@/lib/validations";

export default function AccountsPage() {
  const { data: accounts, isLoading } = useAccounts();
  const createAccountMutation = useCreateAccount();
  const updateAccountMutation = useUpdateAccount();
  const deleteAccountMutation = useDeleteAccount();

  const [isFormModalOpen, setIsFormModalOpen] = useState(false);
  const [isDeleteDialogOpen, setIsDeleteDialogOpen] = useState(false);
  const [selectedAccount, setSelectedAccount] = useState<Account | null>(null);

  // Yeni hesap ekle
  const handleCreate = () => {
    setSelectedAccount(null);
    setIsFormModalOpen(true);
  };

  // Hesap düzenle
  const handleEdit = (account: Account) => {
    setSelectedAccount(account);
    setIsFormModalOpen(true);
  };

  // Hesap sil (dialog aç)
  const handleDelete = (account: Account) => {
    setSelectedAccount(account);
    setIsDeleteDialogOpen(true);
  };

  // Form submit
  const handleFormSubmit = (data: AccountFormData) => {
    if (selectedAccount) {
      // Update
      updateAccountMutation.mutate(
        {
          id: selectedAccount.id,
          data: {
            name: data.name,
            isActive: true,
            includeInTotalBalance: true,
          },
        },
        {
          onSuccess: () => {
            setIsFormModalOpen(false);
            setSelectedAccount(null);
          },
        }
      );
    } else {
      // Create
      createAccountMutation.mutate(data, {
        onSuccess: () => {
          setIsFormModalOpen(false);
        },
      });
    }
  };

  // Delete confirm
  const handleDeleteConfirm = () => {
    if (selectedAccount) {
      deleteAccountMutation.mutate(selectedAccount.id, {
        onSuccess: () => {
          setIsDeleteDialogOpen(false);
          setSelectedAccount(null);
        },
      });
    }
  };

  if (isLoading) {
    return (
      <div className="space-y-6">
        <div className="flex items-center justify-between">
          <Skeleton className="h-10 w-48" />
          <Skeleton className="h-10 w-32" />
        </div>
        <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-3">
          {[1, 2, 3].map((i) => (
            <Skeleton key={i} className="h-64" />
          ))}
        </div>
      </div>
    );
  }

  const accountList = accounts || [];

  return (
    <div className="space-y-6">
      {/* Header */}
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold">Hesaplarım</h1>
          <p className="text-muted-foreground">
            Toplam {accountList.length} hesap
          </p>
        </div>
        <Button onClick={handleCreate}>
          <Plus className="mr-2 h-4 w-4" />
          Yeni Hesap
        </Button>
      </div>

      {/* Empty State */}
      {accountList.length === 0 ? (
        <div className="flex flex-col items-center justify-center py-12 text-center border-2 border-dashed rounded-lg">
          <div className="w-16 h-16 bg-muted rounded-full flex items-center justify-center mb-4">
            <Plus className="w-8 h-8 text-muted-foreground" />
          </div>
          <h3 className="text-lg font-semibold mb-2">
            Henüz hesap eklemediniz
          </h3>
          <p className="text-muted-foreground mb-4 max-w-sm">
            Finansal durumunuzu takip etmek için ilk hesabınızı ekleyin
          </p>
          <Button onClick={handleCreate}>
            <Plus className="mr-2 h-4 w-4" />
            İlk Hesabınızı Ekleyin
          </Button>
        </div>
      ) : (
        /* Account Cards Grid */
        <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-3">
          {accountList.map((account) => (
            <AccountCard
              key={account.id}
              account={account}
              onEdit={handleEdit}
              onDelete={handleDelete}
            />
          ))}
        </div>
      )}

      {/* Form Modal */}
      <AccountFormModal
        open={isFormModalOpen}
        onClose={() => {
          setIsFormModalOpen(false);
          setSelectedAccount(null);
        }}
        onSubmit={handleFormSubmit}
        isLoading={
          createAccountMutation.isPending || updateAccountMutation.isPending
        }
        account={selectedAccount}
      />

      {/* Delete Dialog */}
      <DeleteAccountDialog
        open={isDeleteDialogOpen}
        onClose={() => {
          setIsDeleteDialogOpen(false);
          setSelectedAccount(null);
        }}
        onConfirm={handleDeleteConfirm}
        account={selectedAccount}
        isLoading={deleteAccountMutation.isPending}
      />
    </div>
  );
}
