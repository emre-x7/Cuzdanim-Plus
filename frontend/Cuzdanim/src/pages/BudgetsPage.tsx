import { useState } from "react";
import { Plus } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Skeleton } from "@/components/ui/skeleton";
import { Card, CardContent } from "@/components/ui/card";
import {
  useBudgets,
  useCreateBudget,
  useUpdateBudget,
  useDeleteBudget,
} from "@/hooks/useBudgets";
import { BudgetCard } from "@/components/budgets/BudgetCard";
import { BudgetFormModal } from "@/components/budgets/BudgetFormModal";
import { DeleteBudgetDialog } from "@/components/budgets/DeleteBudgetDialog";
import type { Budget } from "@/types";
import type { BudgetFormData } from "@/lib/validations";

export default function BudgetsPage() {
  // API Queries
  const { data: budgets, isLoading: budgetsLoading } = useBudgets();

  // Mutations
  const createBudgetMutation = useCreateBudget();
  const updateBudgetMutation = useUpdateBudget();
  const deleteBudgetMutation = useDeleteBudget();

  // Modal states
  const [isFormModalOpen, setIsFormModalOpen] = useState(false);
  const [isDeleteDialogOpen, setIsDeleteDialogOpen] = useState(false);
  const [selectedBudget, setSelectedBudget] = useState<Budget | null>(null);

  // Handlers
  const handleCreate = () => {
    setSelectedBudget(null);
    setIsFormModalOpen(true);
  };

  const handleEdit = (budget: Budget) => {
    setSelectedBudget(budget);
    setIsFormModalOpen(true);
  };

  const handleDelete = (budget: Budget) => {
    setSelectedBudget(budget);
    setIsDeleteDialogOpen(true);
  };

  const handleFormSubmit = (data: BudgetFormData) => {
    // Currency mapping (TRY default)
    const currency = 1; // Currency.TRY

    if (selectedBudget) {
      // Update
      updateBudgetMutation.mutate(
        {
          id: selectedBudget.id,
          data: {
            name: data.name,
            amount: data.amount,
            currency: currency,
            startDate: data.startDate,
            endDate: data.endDate,
            alertThresholdPercentage: data.alertThresholdPercentage || 80,
          },
        },
        {
          onSuccess: () => {
            setIsFormModalOpen(false);
            setSelectedBudget(null);
          },
        }
      );
    } else {
      // Create
      createBudgetMutation.mutate(
        {
          name: data.name,
          categoryId: data.categoryId,
          amount: data.amount,
          currency: currency,
          startDate: data.startDate,
          endDate: data.endDate,
          alertThresholdPercentage: data.alertThresholdPercentage || 80,
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
    if (selectedBudget) {
      deleteBudgetMutation.mutate(selectedBudget.id, {
        onSuccess: () => {
          setIsDeleteDialogOpen(false);
          setSelectedBudget(null);
        },
      });
    }
  };

  if (budgetsLoading) {
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

  const budgetList = budgets || [];

  // İstatistikler
  const totalBudget = budgetList.reduce((sum, b) => sum + b.amount, 0);
  const totalSpent = budgetList.reduce((sum, b) => sum + b.spent, 0);
  const exceededCount = budgetList.filter(
    (b) => b.status === "Exceeded"
  ).length;
  const warningCount = budgetList.filter((b) => b.status === "Warning").length;

  return (
    <div className="space-y-6">
      {/* Header */}
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold">Bütçeler</h1>
          <p className="text-muted-foreground">
            Toplam {budgetList.length} bütçe
          </p>
        </div>
        <Button onClick={handleCreate}>
          <Plus className="mr-2 h-4 w-4" />
          Yeni Bütçe
        </Button>
      </div>

      {/* Özet İstatistikler */}
      {budgetList.length > 0 && (
        <Card>
          <CardContent className="p-6">
            <div className="grid gap-6 md:grid-cols-4">
              <div>
                <p className="text-sm text-muted-foreground">Toplam Bütçe</p>
                <p className="text-2xl font-bold">
                  ₺
                  {totalBudget.toLocaleString("tr-TR", {
                    minimumFractionDigits: 2,
                  })}
                </p>
              </div>
              <div>
                <p className="text-sm text-muted-foreground">Harcanan</p>
                <p className="text-2xl font-bold">
                  ₺
                  {totalSpent.toLocaleString("tr-TR", {
                    minimumFractionDigits: 2,
                  })}
                </p>
              </div>
              <div>
                <p className="text-sm text-muted-foreground">Kalan</p>
                <p className="text-2xl font-bold">
                  ₺
                  {(totalBudget - totalSpent).toLocaleString("tr-TR", {
                    minimumFractionDigits: 2,
                  })}
                </p>
              </div>
              {(exceededCount > 0 || warningCount > 0) && (
                <div>
                  <p className="text-sm text-muted-foreground">Uyarılar</p>
                  <div className="flex flex-col gap-1 mt-1">
                    {exceededCount > 0 && (
                      <span className="text-lg font-bold text-red-600">
                        {exceededCount} aşıldı
                      </span>
                    )}
                    {warningCount > 0 && (
                      <span className="text-lg font-bold text-yellow-600">
                        {warningCount} uyarı
                      </span>
                    )}
                  </div>
                </div>
              )}
            </div>
          </CardContent>
        </Card>
      )}

      {/* Empty State */}
      {budgetList.length === 0 ? (
        <div className="flex flex-col items-center justify-center py-12 text-center border-2 border-dashed rounded-lg">
          <div className="w-16 h-16 bg-muted rounded-full flex items-center justify-center mb-4">
            <Plus className="w-8 h-8 text-muted-foreground" />
          </div>
          <h3 className="text-lg font-semibold mb-2">
            Henüz bütçe oluşturmadınız
          </h3>
          <p className="text-muted-foreground mb-4 max-w-sm">
            Kategorileriniz için bütçe belirleyin ve harcamalarınızı takip edin
          </p>
          <Button onClick={handleCreate}>
            <Plus className="mr-2 h-4 w-4" />
            İlk Bütçenizi Oluşturun
          </Button>
        </div>
      ) : (
        /* Budget Cards Grid */
        <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-3">
          {budgetList.map((budget) => (
            <BudgetCard
              key={budget.id}
              budget={budget}
              onEdit={handleEdit}
              onDelete={handleDelete}
            />
          ))}
        </div>
      )}

      {/* Form Modal */}
      <BudgetFormModal
        open={isFormModalOpen}
        onClose={() => {
          setIsFormModalOpen(false);
          setSelectedBudget(null);
        }}
        onSubmit={handleFormSubmit}
        isLoading={
          createBudgetMutation.isPending || updateBudgetMutation.isPending
        }
        budget={selectedBudget}
      />

      {/* Delete Dialog */}
      <DeleteBudgetDialog
        open={isDeleteDialogOpen}
        onClose={() => {
          setIsDeleteDialogOpen(false);
          setSelectedBudget(null);
        }}
        onConfirm={handleDeleteConfirm}
        budget={selectedBudget}
        isLoading={deleteBudgetMutation.isPending}
      />
    </div>
  );
}
