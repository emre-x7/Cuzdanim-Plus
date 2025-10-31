import { useEffect } from "react";
import { useForm, type SubmitHandler } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { Loader2 } from "lucide-react";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { budgetSchema, type BudgetFormData } from "@/lib/validations";
import { useCategories } from "@/hooks/useCategories";
import type { Budget } from "@/types";

interface BudgetFormModalProps {
  open: boolean;
  onClose: () => void;
  onSubmit: (data: BudgetFormData) => void;
  isLoading: boolean;
  budget?: Budget | null;
}

export function BudgetFormModal({
  open,
  onClose,
  onSubmit,
  isLoading,
  budget,
}: BudgetFormModalProps) {
  const isEditMode = !!budget;

  // Kategorileri çek (sadece Expense kategorileri)
  const { data: allCategories } = useCategories();
  const expenseCategories = (allCategories || []).filter(
    (cat) => cat.transactionType === "Expense"
  );

  const form = useForm<BudgetFormData>({
    resolver: zodResolver(budgetSchema),
    defaultValues: {
      name: "",
      categoryId: "",
      amount: 0,
      startDate: new Date().toISOString().split("T")[0], // YYYY-MM-DD
      endDate: new Date(new Date().setMonth(new Date().getMonth() + 1))
        .toISOString()
        .split("T")[0],
      alertThresholdPercentage: 80,
    },
  });

  const {
    register,
    handleSubmit,
    setValue,
    watch,
    reset,
    formState: { errors },
  } = form;
  const selectedCategoryId = watch("categoryId");

  // Edit modunda form'u doldur
  useEffect(() => {
    if (budget) {
      setValue("name", budget.name);
      setValue("categoryId", budget.categoryId);
      setValue("amount", budget.amount);

      //  Defensive: periodStartDate null olabilir
      if (budget.periodStartDate) {
        setValue("startDate", budget.periodStartDate.split("T")[0]);
      }

      // Defensive: periodEndDate null olabilir
      if (budget.periodEndDate) {
        setValue("endDate", budget.periodEndDate.split("T")[0]);
      }

      setValue("alertThresholdPercentage", budget.alertThresholdPercentage);
    } else {
      reset();
    }
  }, [budget, setValue, reset]);

  const onSubmitHandler: SubmitHandler<BudgetFormData> = (data) => {
    onSubmit(data);
  };

  return (
    <Dialog open={open} onOpenChange={onClose}>
      <DialogContent className="sm:max-w-[500px] max-h-[90vh] overflow-y-auto">
        <DialogHeader>
          <DialogTitle>
            {isEditMode ? "Bütçe Düzenle" : "Yeni Bütçe Oluştur"}
          </DialogTitle>
          <DialogDescription>
            {isEditMode
              ? "Bütçe bilgilerini güncelleyin"
              : "Bir kategori için bütçe belirleyin"}
          </DialogDescription>
        </DialogHeader>

        <form onSubmit={handleSubmit(onSubmitHandler)} className="space-y-4">
          {/* Bütçe Adı */}
          <div className="space-y-2">
            <Label htmlFor="name">Bütçe Adı *</Label>
            <Input
              id="name"
              placeholder="Ocak 2025 Yemek Bütçesi"
              {...register("name")}
              disabled={isLoading}
            />
            {errors.name && (
              <p className="text-sm text-destructive">{errors.name.message}</p>
            )}
          </div>

          {/* Kategori */}
          <div className="space-y-2">
            <Label htmlFor="categoryId">Kategori *</Label>
            <Select
              value={selectedCategoryId}
              onValueChange={(value) =>
                setValue("categoryId", value, { shouldValidate: true })
              }
              disabled={isLoading || isEditMode}
            >
              <SelectTrigger>
                <SelectValue placeholder="Kategori seçin" />
              </SelectTrigger>
              <SelectContent>
                {expenseCategories.length === 0 ? (
                  <div className="p-2 text-sm text-muted-foreground text-center">
                    Gider kategorisi bulunamadı
                  </div>
                ) : (
                  expenseCategories.map((category) => (
                    <SelectItem key={category.id} value={category.id}>
                      <div className="flex items-center gap-2">
                        {category.icon && (
                          <span style={{ color: category.color || undefined }}>
                            {category.icon}
                          </span>
                        )}
                        <span>{category.name}</span>
                      </div>
                    </SelectItem>
                  ))
                )}
              </SelectContent>
            </Select>
            {errors.categoryId && (
              <p className="text-sm text-destructive">
                {errors.categoryId.message}
              </p>
            )}
          </div>

          {/* Bütçe Tutarı */}
          <div className="space-y-2">
            <Label htmlFor="amount">Bütçe Tutarı *</Label>
            <Input
              id="amount"
              type="number"
              step="0.01"
              placeholder="0.00"
              {...register("amount", { valueAsNumber: true })}
              disabled={isLoading}
            />
            {errors.amount && (
              <p className="text-sm text-destructive">
                {errors.amount.message}
              </p>
            )}
          </div>

          {/* Başlangıç ve Bitiş Tarihleri */}
          <div className="grid grid-cols-2 gap-4">
            <div className="space-y-2">
              <Label htmlFor="startDate">Başlangıç Tarihi *</Label>
              <Input
                id="startDate"
                type="date"
                {...register("startDate")}
                disabled={isLoading}
              />
              {errors.startDate && (
                <p className="text-sm text-destructive">
                  {errors.startDate.message}
                </p>
              )}
            </div>

            <div className="space-y-2">
              <Label htmlFor="endDate">Bitiş Tarihi *</Label>
              <Input
                id="endDate"
                type="date"
                {...register("endDate")}
                disabled={isLoading}
              />
              {errors.endDate && (
                <p className="text-sm text-destructive">
                  {errors.endDate.message}
                </p>
              )}
            </div>
          </div>

          {/* Uyarı Eşiği */}
          <div className="space-y-2">
            <Label htmlFor="alertThresholdPercentage">Uyarı Eşiği (%)</Label>
            <Input
              id="alertThresholdPercentage"
              type="number"
              min="0"
              max="100"
              placeholder="80"
              {...register("alertThresholdPercentage", { valueAsNumber: true })}
              disabled={isLoading}
            />
            {errors.alertThresholdPercentage && (
              <p className="text-sm text-destructive">
                {errors.alertThresholdPercentage.message}
              </p>
            )}
            <p className="text-xs text-muted-foreground">
              Bütçenin bu yüzdesine ulaşıldığında uyarı verilir
            </p>
          </div>

          <DialogFooter>
            <Button
              type="button"
              variant="outline"
              onClick={onClose}
              disabled={isLoading}
            >
              İptal
            </Button>
            <Button type="submit" disabled={isLoading}>
              {isLoading ? (
                <>
                  <Loader2 className="mr-2 h-4 w-4 animate-spin" />
                  Kaydediliyor...
                </>
              ) : isEditMode ? (
                "Güncelle"
              ) : (
                "Oluştur"
              )}
            </Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  );
}
