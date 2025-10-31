import { useEffect } from "react";
import { useForm, type SubmitHandler } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { format } from "date-fns";
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
import { Textarea } from "@/components/ui/textarea";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { transactionSchema, type TransactionFormData } from "@/lib/validations";
import { TransactionType } from "@/types";
import { useCategories } from "@/hooks/useCategories";
import { formatCurrency } from "@/lib/utils";
import type { Transaction, Account } from "@/types";

interface TransactionFormModalProps {
  open: boolean;
  onClose: () => void;
  onSubmit: (data: TransactionFormData) => void;
  isLoading: boolean;
  transaction?: Transaction | null;
  accounts: Account[];
}

export function TransactionFormModal({
  open,
  onClose,
  onSubmit,
  isLoading,
  transaction,
  accounts,
}: TransactionFormModalProps) {
  const isEditMode = !!transaction;

  // Backend'den kategorileri çek
  const { data: categories, isLoading: categoriesLoading } = useCategories();
  const allCategories = categories || [];

  const form = useForm<TransactionFormData>({
    resolver: zodResolver(transactionSchema),
    defaultValues: {
      type: TransactionType.Expense,
      amount: 0,
      accountId: "",
      categoryId: "",
      transactionDate: format(new Date(), "yyyy-MM-dd"),
      description: "",
      notes: "",
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
  const selectedType = watch("type");
  const selectedAccountId = watch("accountId");
  const selectedCategoryId = watch("categoryId");

  // Kategorileri filtrele (transactionType field'ını kullan)
  const availableCategories = allCategories.filter((cat) => {
    const isIncome = selectedType === 1;
    const expectedType = isIncome ? "Income" : "Expense";

    return cat.transactionType === expectedType;
  });

  // Edit modunda form'u doldur
  useEffect(() => {
    if (transaction) {
      const typeValue =
        transaction.type === "Income"
          ? TransactionType.Income
          : TransactionType.Expense;

      setValue("type", typeValue);
      setValue("amount", transaction.amount);
      setValue("accountId", transaction.accountId);
      setValue("categoryId", transaction.categoryId);
      setValue(
        "transactionDate",
        format(new Date(transaction.transactionDate), "yyyy-MM-dd")
      );
      setValue("description", transaction.description || "");
      setValue("notes", transaction.notes || "");
    } else {
      reset();
    }
  }, [transaction, setValue, reset]);

  // Type değişince kategori resetle (edit mode değilse)
  useEffect(() => {
    if (!isEditMode) {
      setValue("categoryId", "");
    }
  }, [selectedType, isEditMode, setValue]);

  const onSubmitHandler: SubmitHandler<TransactionFormData> = (data) => {
    onSubmit(data);
  };

  return (
    <Dialog open={open} onOpenChange={onClose}>
      <DialogContent className="sm:max-w-[500px] max-h-[90vh] overflow-y-auto">
        <DialogHeader>
          <DialogTitle>
            {isEditMode ? "İşlem Düzenle" : "Yeni İşlem Ekle"}
          </DialogTitle>
          <DialogDescription>
            {isEditMode
              ? "İşlem bilgilerini güncelleyin"
              : "Yeni bir gelir veya gider ekleyin"}
          </DialogDescription>
        </DialogHeader>

        <form onSubmit={handleSubmit(onSubmitHandler)} className="space-y-4">
          {/* İşlem Tipi */}
          <div className="space-y-2">
            <Label htmlFor="type">İşlem Tipi *</Label>
            <Select
              value={selectedType?.toString()}
              onValueChange={(value) =>
                setValue("type", parseInt(value), { shouldValidate: true })
              }
              disabled={isLoading || isEditMode}
            >
              <SelectTrigger>
                <SelectValue placeholder="İşlem tipi seçin" />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value={TransactionType.Income.toString()}>
                  ✅ Gelir
                </SelectItem>
                <SelectItem value={TransactionType.Expense.toString()}>
                  ❌ Gider
                </SelectItem>
              </SelectContent>
            </Select>
            {errors.type && (
              <p className="text-sm text-destructive">{errors.type.message}</p>
            )}
          </div>

          {/* Tutar */}
          <div className="space-y-2">
            <Label htmlFor="amount">Tutar *</Label>
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

          {/* Hesap */}
          <div className="space-y-2">
            <Label htmlFor="accountId">Hesap *</Label>
            <Select
              value={selectedAccountId}
              onValueChange={(value) =>
                setValue("accountId", value, { shouldValidate: true })
              }
              disabled={isLoading || isEditMode}
            >
              <SelectTrigger>
                <SelectValue placeholder="Hesap seçin" />
              </SelectTrigger>
              <SelectContent>
                {accounts.length === 0 ? (
                  <div className="p-2 text-sm text-muted-foreground text-center">
                    Hesap bulunamadı
                  </div>
                ) : (
                  accounts.map((account) => (
                    <SelectItem key={account.id} value={account.id}>
                      {account.name} (
                      {formatCurrency(account.balance, account.currency)})
                    </SelectItem>
                  ))
                )}
              </SelectContent>
            </Select>
            {errors.accountId && (
              <p className="text-sm text-destructive">
                {errors.accountId.message}
              </p>
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
              disabled={isLoading || categoriesLoading}
            >
              <SelectTrigger>
                <SelectValue
                  placeholder={
                    categoriesLoading ? "Yükleniyor..." : "Kategori seçin"
                  }
                />
              </SelectTrigger>
              <SelectContent>
                {categoriesLoading ? (
                  <div className="p-2 text-sm text-muted-foreground text-center">
                    Yükleniyor...
                  </div>
                ) : availableCategories.length === 0 ? (
                  <div className="p-2 text-sm text-muted-foreground text-center">
                    Bu tip için kategori bulunamadı
                  </div>
                ) : (
                  availableCategories.map((category) => (
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

          {/* İşlem Tarihi */}
          <div className="space-y-2">
            <Label htmlFor="transactionDate">İşlem Tarihi *</Label>
            <Input
              id="transactionDate"
              type="date"
              {...register("transactionDate")}
              disabled={isLoading}
            />
            {errors.transactionDate && (
              <p className="text-sm text-destructive">
                {errors.transactionDate.message}
              </p>
            )}
          </div>

          {/* Açıklama */}
          <div className="space-y-2">
            <Label htmlFor="description">
              Açıklama{" "}
              <span className="text-muted-foreground">(Opsiyonel)</span>
            </Label>
            <Input
              id="description"
              placeholder="Market alışverişi"
              {...register("description")}
              disabled={isLoading}
            />
          </div>

          {/* Notlar */}
          <div className="space-y-2">
            <Label htmlFor="notes">
              Notlar <span className="text-muted-foreground">(Opsiyonel)</span>
            </Label>
            <Textarea
              id="notes"
              placeholder="Ek bilgiler..."
              rows={3}
              {...register("notes")}
              disabled={isLoading}
            />
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
            <Button type="submit" disabled={isLoading || categoriesLoading}>
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
