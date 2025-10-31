import { useState } from "react";
import { Plus } from "lucide-react";
import { format } from "date-fns";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Skeleton } from "@/components/ui/skeleton";
import { Card, CardContent } from "@/components/ui/card";
import {
  useTransactions,
  useCreateTransaction,
  useUpdateTransaction,
  useDeleteTransaction,
} from "@/hooks/useTransactions";
import { useAccounts } from "@/hooks/useAccounts";
import { TransactionCard } from "@/components/transactions/TransactionCard";
import { TransactionFormModal } from "@/components/transactions/TransactionFormModal";
import { DeleteTransactionDialog } from "@/components/transactions/DeleteTransactionDialog";
import type { Transaction } from "@/types";
import type { TransactionFormData } from "@/lib/validations";

export default function TransactionsPage() {
  // Date filters (bu ay default)
  const today = new Date();
  const firstDayOfMonth = new Date(today.getFullYear(), today.getMonth(), 1);
  const lastDayOfMonth = new Date(today.getFullYear(), today.getMonth() + 1, 0);

  const [startDate, setStartDate] = useState(
    format(firstDayOfMonth, "yyyy-MM-dd")
  );
  const [endDate, setEndDate] = useState(format(lastDayOfMonth, "yyyy-MM-dd"));

  // API Queries
  const { data: transactions, isLoading: transactionsLoading } =
    useTransactions({
      startDate,
      endDate,
    });
  const { data: accounts } = useAccounts();

  // Mutations
  const createTransactionMutation = useCreateTransaction();
  const updateTransactionMutation = useUpdateTransaction();
  const deleteTransactionMutation = useDeleteTransaction();

  // Modal states
  const [isFormModalOpen, setIsFormModalOpen] = useState(false);
  const [isDeleteDialogOpen, setIsDeleteDialogOpen] = useState(false);
  const [selectedTransaction, setSelectedTransaction] =
    useState<Transaction | null>(null);

  // Handlers
  const handleCreate = () => {
    setSelectedTransaction(null);
    setIsFormModalOpen(true);
  };

  const handleEdit = (transaction: Transaction) => {
    setSelectedTransaction(transaction);
    setIsFormModalOpen(true);
  };

  const handleDelete = (transaction: Transaction) => {
    setSelectedTransaction(transaction);
    setIsDeleteDialogOpen(true);
  };

  const handleFormSubmit = (data: TransactionFormData) => {
    // Seçili hesabın currency'sini al ve number'a çevir
    const selectedAccount = accountList.find(
      (acc) => acc.id === data.accountId
    );

    // Currency mapping (string -> number)
    const currencyMap: Record<string, number> = {
      TRY: 1,
      USD: 2,
      EUR: 3,
      GBP: 4,
      GOLD: 5,
    };

    const currencyValue = selectedAccount
      ? currencyMap[selectedAccount.currency] || 1
      : 1;

    if (selectedTransaction) {
      // Update
      updateTransactionMutation.mutate(
        {
          id: selectedTransaction.id,
          data: {
            amount: data.amount,
            categoryId: data.categoryId,
            transactionDate: data.transactionDate,
            description: data.description,
            notes: data.notes,
            currency: currencyValue,
          },
        },
        {
          onSuccess: () => {
            setIsFormModalOpen(false);
            setSelectedTransaction(null);
          },
        }
      );
    } else {
      // Create
      createTransactionMutation.mutate(
        {
          type: data.type,
          amount: data.amount,
          accountId: data.accountId,
          categoryId: data.categoryId,
          transactionDate: data.transactionDate,
          currency: currencyValue,
          description: data.description,
          notes: data.notes,
        },
        {
          onSuccess: () => {
            setIsFormModalOpen(false);
          },
        }
      );
    }
  };

  const handleDeleteConfirm = () => {
    if (selectedTransaction) {
      deleteTransactionMutation.mutate(selectedTransaction.id, {
        onSuccess: () => {
          setIsDeleteDialogOpen(false);
          setSelectedTransaction(null);
        },
      });
    }
  };

  if (transactionsLoading) {
    return (
      <div className="space-y-6">
        <div className="flex items-center justify-between">
          <Skeleton className="h-10 w-48" />
          <Skeleton className="h-10 w-32" />
        </div>
        <div className="space-y-4">
          {[1, 2, 3, 4, 5].map((i) => (
            <Skeleton key={i} className="h-24" />
          ))}
        </div>
      </div>
    );
  }

  const transactionList = transactions || [];
  const accountList = accounts || [];

  return (
    <div className="space-y-6">
      {/* Header */}
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold">İşlemler</h1>
          <p className="text-muted-foreground">
            Toplam {transactionList.length} işlem
          </p>
        </div>
        <Button onClick={handleCreate}>
          <Plus className="mr-2 h-4 w-4" />
          Yeni İşlem
        </Button>
      </div>

      {/* Filters */}
      <Card>
        <CardContent className="p-4">
          <div className="grid gap-4 md:grid-cols-2">
            <div className="space-y-2">
              <Label htmlFor="startDate">Başlangıç Tarihi</Label>
              <Input
                id="startDate"
                type="date"
                value={startDate}
                onChange={(e) => setStartDate(e.target.value)}
              />
            </div>
            <div className="space-y-2">
              <Label htmlFor="endDate">Bitiş Tarihi</Label>
              <Input
                id="endDate"
                type="date"
                value={endDate}
                onChange={(e) => setEndDate(e.target.value)}
              />
            </div>
          </div>
        </CardContent>
      </Card>

      {/* Empty State */}
      {transactionList.length === 0 ? (
        <div className="flex flex-col items-center justify-center py-12 text-center border-2 border-dashed rounded-lg">
          <div className="w-16 h-16 bg-muted rounded-full flex items-center justify-center mb-4">
            <Plus className="w-8 h-8 text-muted-foreground" />
          </div>
          <h3 className="text-lg font-semibold mb-2">
            Bu tarih aralığında işlem bulunamadı
          </h3>
          <p className="text-muted-foreground mb-4 max-w-sm">
            Gelir veya gider eklemek için "Yeni İşlem" butonuna tıklayın
          </p>
          <Button onClick={handleCreate}>
            <Plus className="mr-2 h-4 w-4" />
            İlk İşleminizi Ekleyin
          </Button>
        </div>
      ) : (
        /* Transaction List */
        <div className="space-y-4">
          {transactionList.map((transaction) => (
            <TransactionCard
              key={transaction.id}
              transaction={transaction}
              onEdit={handleEdit}
              onDelete={handleDelete}
            />
          ))}
        </div>
      )}

      {/* Form Modal */}
      <TransactionFormModal
        open={isFormModalOpen}
        onClose={() => {
          setIsFormModalOpen(false);
          setSelectedTransaction(null);
        }}
        onSubmit={handleFormSubmit}
        isLoading={
          createTransactionMutation.isPending ||
          updateTransactionMutation.isPending
        }
        transaction={selectedTransaction}
        accounts={accountList}
      />

      {/* Delete Dialog */}
      <DeleteTransactionDialog
        open={isDeleteDialogOpen}
        onClose={() => {
          setIsDeleteDialogOpen(false);
          setSelectedTransaction(null);
        }}
        onConfirm={handleDeleteConfirm}
        transaction={selectedTransaction}
        isLoading={deleteTransactionMutation.isPending}
      />
    </div>
  );
}
