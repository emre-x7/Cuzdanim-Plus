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
import { Textarea } from "@/components/ui/textarea";
import { goalSchema, type GoalFormData } from "@/lib/validations";
import type { Goal } from "@/types";

interface GoalFormModalProps {
  open: boolean;
  onClose: () => void;
  onSubmit: (data: GoalFormData) => void;
  isLoading: boolean;
  goal?: Goal | null;
}

export function GoalFormModal({
  open,
  onClose,
  onSubmit,
  isLoading,
  goal,
}: GoalFormModalProps) {
  const isEditMode = !!goal;

  const form = useForm<GoalFormData>({
    resolver: zodResolver(goalSchema),
    defaultValues: {
      name: "",
      description: "",
      targetAmount: 0,
      targetDate: new Date(new Date().setMonth(new Date().getMonth() + 6))
        .toISOString()
        .split("T")[0],
      icon: "🎯",
    },
  });

  const {
    register,
    handleSubmit,
    setValue,
    reset,
    formState: { errors },
  } = form;

  // Edit modunda form'u doldur
  useEffect(() => {
    if (goal) {
      setValue("name", goal.name);
      setValue("description", goal.description || "");
      setValue("targetAmount", goal.targetAmount);
      if (goal.targetDate) {
        setValue("targetDate", goal.targetDate.split("T")[0]);
      }
      setValue("icon", goal.icon || "🎯");
    } else {
      reset();
    }
  }, [goal, setValue, reset]);

  const onSubmitHandler: SubmitHandler<GoalFormData> = (data) => {
    onSubmit(data);
  };

  return (
    <Dialog open={open} onOpenChange={onClose}>
      <DialogContent className="sm:max-w-[500px] max-h-[90vh] overflow-y-auto">
        <DialogHeader>
          <DialogTitle>
            {isEditMode ? "Hedef Düzenle" : "Yeni Hedef Oluştur"}
          </DialogTitle>
          <DialogDescription>
            {isEditMode
              ? "Hedef bilgilerini güncelleyin"
              : "Tasarruf hedefi oluşturun ve katkı ekleyin"}
          </DialogDescription>
        </DialogHeader>

        <form onSubmit={handleSubmit(onSubmitHandler)} className="space-y-4">
          {/* Hedef Adı */}
          <div className="space-y-2">
            <Label htmlFor="name">Hedef Adı *</Label>
            <Input
              id="name"
              placeholder="Tatil için birikim"
              {...register("name")}
              disabled={isLoading}
            />
            {errors.name && (
              <p className="text-sm text-destructive">{errors.name.message}</p>
            )}
          </div>

          {/* Açıklama */}
          <div className="space-y-2">
            <Label htmlFor="description">
              Açıklama{" "}
              <span className="text-muted-foreground">(Opsiyonel)</span>
            </Label>
            <Textarea
              id="description"
              placeholder="Yaz tatili için İtalya gezisi..."
              rows={3}
              {...register("description")}
              disabled={isLoading}
            />
            {errors.description && (
              <p className="text-sm text-destructive">
                {errors.description.message}
              </p>
            )}
          </div>

          {/* Hedef Tutar */}
          <div className="space-y-2">
            <Label htmlFor="targetAmount">Hedef Tutar *</Label>
            <Input
              id="targetAmount"
              type="number"
              step="0.01"
              placeholder="10000.00"
              {...register("targetAmount", { valueAsNumber: true })}
              disabled={isLoading}
            />
            {errors.targetAmount && (
              <p className="text-sm text-destructive">
                {errors.targetAmount.message}
              </p>
            )}
          </div>

          {/* Hedef Tarihi */}
          <div className="space-y-2">
            <Label htmlFor="targetDate">Hedef Tarihi *</Label>
            <Input
              id="targetDate"
              type="date"
              {...register("targetDate")}
              disabled={isLoading}
            />
            {errors.targetDate && (
              <p className="text-sm text-destructive">
                {errors.targetDate.message}
              </p>
            )}
            <p className="text-xs text-muted-foreground">
              Bu tarihe kadar hedefe ulaşmayı planlıyorsunuz
            </p>
          </div>

          {/* Icon */}
          <div className="space-y-2">
            <Label htmlFor="icon">
              Emoji İkonu{" "}
              <span className="text-muted-foreground">(Opsiyonel)</span>
            </Label>
            <Input
              id="icon"
              placeholder="🎯"
              maxLength={2}
              {...register("icon")}
              disabled={isLoading}
            />
            <p className="text-xs text-muted-foreground">
              Örnek: 🏖️, 🏠, 🚗, 💍, 🎓, 💰
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
