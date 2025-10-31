import { useState } from "react";
import { Plus } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Skeleton } from "@/components/ui/skeleton";
import { Card, CardContent } from "@/components/ui/card";
import { Tabs, TabsList, TabsTrigger } from "@/components/ui/tabs";
import {
  useGoals,
  useCreateGoal,
  useUpdateGoal,
  useDeleteGoal,
  useAddContribution,
} from "@/hooks/useGoals";
import { GoalCard } from "@/components/goals/GoalCard";
import { GoalFormModal } from "@/components/goals/GoalFormModal";
import { AddContributionModal } from "@/components/goals/AddContributionModal";
import { DeleteGoalDialog } from "@/components/goals/DeleteGoalDialog";
import type { Goal } from "@/types";
import type { GoalFormData, AddContributionFormData } from "@/lib/validations";

type StatusFilter = "all" | "active" | "completed";

export default function GoalsPage() {
  // Filter state
  const [statusFilter, setStatusFilter] = useState<StatusFilter>("all");

  // API Queries
  const { data: goals, isLoading: goalsLoading } = useGoals();

  // Mutations
  const createGoalMutation = useCreateGoal();
  const updateGoalMutation = useUpdateGoal();
  const deleteGoalMutation = useDeleteGoal();
  const addContributionMutation = useAddContribution();

  // Modal states
  const [isFormModalOpen, setIsFormModalOpen] = useState(false);
  const [isContributionModalOpen, setIsContributionModalOpen] = useState(false);
  const [isDeleteDialogOpen, setIsDeleteDialogOpen] = useState(false);
  const [selectedGoal, setSelectedGoal] = useState<Goal | null>(null);

  // Handlers
  const handleCreate = () => {
    setSelectedGoal(null);
    setIsFormModalOpen(true);
  };

  const handleEdit = (goal: Goal) => {
    setSelectedGoal(goal);
    setIsFormModalOpen(true);
  };

  const handleDelete = (goal: Goal) => {
    setSelectedGoal(goal);
    setIsDeleteDialogOpen(true);
  };

  const handleAddContribution = (goal: Goal) => {
    setSelectedGoal(goal);
    setIsContributionModalOpen(true);
  };

  const handleFormSubmit = (data: GoalFormData) => {
    // Currency mapping (TRY default)
    const currency = 1; // Currency.TRY

    if (selectedGoal) {
      // Update
      updateGoalMutation.mutate(
        {
          id: selectedGoal.id,
          data: {
            name: data.name,
            description: data.description,
            targetAmount: data.targetAmount,
            currency: currency,
            targetDate: data.targetDate,
            icon: data.icon,
          },
        },
        {
          onSuccess: () => {
            setIsFormModalOpen(false);
            setSelectedGoal(null);
          },
        }
      );
    } else {
      // Create
      createGoalMutation.mutate(
        {
          name: data.name,
          description: data.description,
          targetAmount: data.targetAmount,
          currency: currency,
          targetDate: data.targetDate,
          icon: data.icon,
        },
        {
          onSuccess: () => {
            setIsFormModalOpen(false);
          },
        }
      );
    }
  };

  const handleContributionSubmit = (data: AddContributionFormData) => {
    if (selectedGoal) {
      addContributionMutation.mutate(
        {
          id: selectedGoal.id,
          data: {
            amount: data.amount,
            note: data.note,
            currency: 1,
          },
        },
        {
          onSuccess: () => {
            setIsContributionModalOpen(false);
            setSelectedGoal(null);
          },
        }
      );
    }
  };

  const handleDeleteConfirm = () => {
    if (selectedGoal) {
      deleteGoalMutation.mutate(selectedGoal.id, {
        onSuccess: () => {
          setIsDeleteDialogOpen(false);
          setSelectedGoal(null);
        },
      });
    }
  };

  if (goalsLoading) {
    return (
      <div className="space-y-6">
        <div className="flex items-center justify-between">
          <Skeleton className="h-10 w-48" />
          <Skeleton className="h-10 w-32" />
        </div>
        <Skeleton className="h-12 w-full" />
        <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-3">
          {[1, 2, 3].map((i) => (
            <Skeleton key={i} className="h-72" />
          ))}
        </div>
      </div>
    );
  }

  const goalList = goals || [];

  // Filter goals
  const filteredGoals = goalList.filter((goal) => {
    if (statusFilter === "all") return true;
    if (statusFilter === "active") return goal.status === "Active";
    if (statusFilter === "completed") return goal.status === "Completed";
    return true;
  });

  // İstatistikler
  const totalGoals = goalList.length;
  const activeGoals = goalList.filter((g) => g.status === "Active").length;
  const completedGoals = goalList.filter(
    (g) => g.status === "Completed"
  ).length;
  const totalTarget = goalList.reduce((sum, g) => sum + g.targetAmount, 0);
  const totalCurrent = goalList.reduce((sum, g) => sum + g.currentAmount, 0);

  return (
    <div className="space-y-6">
      {/* Header */}
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold">Hedefler</h1>
          <p className="text-muted-foreground">Toplam {totalGoals} hedef</p>
        </div>
        <Button onClick={handleCreate}>
          <Plus className="mr-2 h-4 w-4" />
          Yeni Hedef
        </Button>
      </div>

      {/* Özet İstatistikler */}
      {goalList.length > 0 && (
        <Card>
          <CardContent className="p-6">
            <div className="grid gap-6 md:grid-cols-4">
              <div>
                <p className="text-sm text-muted-foreground">Aktif Hedefler</p>
                <p className="text-2xl font-bold">{activeGoals}</p>
              </div>
              <div>
                <p className="text-sm text-muted-foreground">Tamamlanan</p>
                <p className="text-2xl font-bold text-green-600">
                  {completedGoals}
                </p>
              </div>
              <div>
                <p className="text-sm text-muted-foreground">Toplam Hedef</p>
                <p className="text-2xl font-bold">
                  ₺
                  {totalTarget.toLocaleString("tr-TR", {
                    minimumFractionDigits: 2,
                  })}
                </p>
              </div>
              <div>
                <p className="text-sm text-muted-foreground">Biriken</p>
                <p className="text-2xl font-bold text-primary">
                  ₺
                  {totalCurrent.toLocaleString("tr-TR", {
                    minimumFractionDigits: 2,
                  })}
                </p>
              </div>
            </div>
          </CardContent>
        </Card>
      )}

      {/* Status Filter */}
      {goalList.length > 0 && (
        <Tabs
          value={statusFilter}
          onValueChange={(v) => setStatusFilter(v as StatusFilter)}
        >
          <TabsList>
            <TabsTrigger value="all">Tümü ({totalGoals})</TabsTrigger>
            <TabsTrigger value="active">Aktif ({activeGoals})</TabsTrigger>
            <TabsTrigger value="completed">
              Tamamlanan ({completedGoals})
            </TabsTrigger>
          </TabsList>
        </Tabs>
      )}

      {/* Empty State */}
      {goalList.length === 0 ? (
        <div className="flex flex-col items-center justify-center py-12 text-center border-2 border-dashed rounded-lg">
          <div className="w-16 h-16 bg-muted rounded-full flex items-center justify-center mb-4">
            <Plus className="w-8 h-8 text-muted-foreground" />
          </div>
          <h3 className="text-lg font-semibold mb-2">
            Henüz hedef oluşturmadınız
          </h3>
          <p className="text-muted-foreground mb-4 max-w-sm">
            Tasarruf hedefleri oluşturun ve katkı ekleyerek hedefinize ulaşın
          </p>
          <Button onClick={handleCreate}>
            <Plus className="mr-2 h-4 w-4" />
            İlk Hedefinizi Oluşturun
          </Button>
        </div>
      ) : filteredGoals.length === 0 ? (
        <div className="text-center py-12">
          <p className="text-muted-foreground">Bu filtrede hedef bulunamadı</p>
        </div>
      ) : (
        /* Goal Cards Grid */
        <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-3">
          {filteredGoals.map((goal) => (
            <GoalCard
              key={goal.id}
              goal={goal}
              onEdit={handleEdit}
              onDelete={handleDelete}
              onAddContribution={handleAddContribution}
            />
          ))}
        </div>
      )}

      {/* Form Modal */}
      <GoalFormModal
        open={isFormModalOpen}
        onClose={() => {
          setIsFormModalOpen(false);
          setSelectedGoal(null);
        }}
        onSubmit={handleFormSubmit}
        isLoading={createGoalMutation.isPending || updateGoalMutation.isPending}
        goal={selectedGoal}
      />

      {/* Add Contribution Modal */}
      <AddContributionModal
        open={isContributionModalOpen}
        onClose={() => {
          setIsContributionModalOpen(false);
          setSelectedGoal(null);
        }}
        onSubmit={handleContributionSubmit}
        isLoading={addContributionMutation.isPending}
        goal={selectedGoal}
      />

      {/* Delete Dialog */}
      <DeleteGoalDialog
        open={isDeleteDialogOpen}
        onClose={() => {
          setIsDeleteDialogOpen(false);
          setSelectedGoal(null);
        }}
        onConfirm={handleDeleteConfirm}
        goal={selectedGoal}
        isLoading={deleteGoalMutation.isPending}
      />
    </div>
  );
}
