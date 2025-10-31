import { useForm, type SubmitHandler } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { Loader2, TrendingUp } from "lucide-react";
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
  addContributionSchema,
  type AddContributionFormData,
} from "@/lib/validations";
import { formatCurrency } from "@/lib/utils";
import type { Goal } from "@/types";

interface AddContributionModalProps {
  open: boolean;
  onClose: () => void;
  onSubmit: (data: AddContributionFormData) => void;
  isLoading: boolean;
  goal: Goal | null;
}

export function AddContributionModal({
  open,
  onClose,
  onSubmit,
  isLoading,
  goal,
}: AddContributionModalProps) {
  const form = useForm<AddContributionFormData>({
    resolver: zodResolver(addContributionSchema),
    defaultValues: {
      amount: 0,
      note: "",
    },
  });

  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = form;

  const onSubmitHandler: SubmitHandler<AddContributionFormData> = (data) => {
    onSubmit(data);
    reset();
  };

  if (!goal) return null;

  return (
    <Dialog open={open} onOpenChange={onClose}>
      <DialogContent className="sm:max-w-[450px]">
        <DialogHeader>
          <DialogTitle>Katkı Ekle</DialogTitle>
          <DialogDescription>
            <span className="font-semibold">{goal.name}</span> hedefinize katkı
            ekleyin
          </DialogDescription>
        </DialogHeader>

        <form onSubmit={handleSubmit(onSubmitHandler)} className="space-y-4">
          {/* Hedef Özet */}
          <div className="bg-muted p-4 rounded-lg space-y-2">
            <div className="flex items-center justify-between text-sm">
              <span className="text-muted-foreground">Mevcut</span>
              <span className="font-semibold">
                {formatCurrency(goal.currentAmount, goal.currency)}
              </span>
            </div>
            <div className="flex items-center justify-between text-sm">
              <span className="text-muted-foreground">Hedef</span>
              <span className="font-semibold">
                {formatCurrency(goal.targetAmount, goal.currency)}
              </span>
            </div>
            <div className="flex items-center justify-between text-sm">
              <span className="text-muted-foreground">Kalan</span>
              <span className="font-semibold text-primary">
                {formatCurrency(goal.remainingAmount, goal.currency)}
              </span>
            </div>
          </div>

          {/* Katkı Tutarı */}
          <div className="space-y-2">
            <Label htmlFor="amount">Katkı Tutarı *</Label>
            <div className="relative">
              <TrendingUp className="absolute left-3 top-3 h-4 w-4 text-muted-foreground" />
              <Input
                id="amount"
                type="number"
                step="0.01"
                placeholder="0.00"
                className="pl-10"
                {...register("amount", { valueAsNumber: true })}
                disabled={isLoading}
              />
            </div>
            {errors.amount && (
              <p className="text-sm text-destructive">
                {errors.amount.message}
              </p>
            )}
          </div>

          {/* Not */}
          <div className="space-y-2">
            <Label htmlFor="note">
              Not <span className="text-muted-foreground">(Opsiyonel)</span>
            </Label>
            <Textarea
              id="note"
              placeholder="Maaş ödemesinden..."
              rows={2}
              {...register("note")}
              disabled={isLoading}
            />
            {errors.note && (
              <p className="text-sm text-destructive">{errors.note.message}</p>
            )}
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
                  Ekleniyor...
                </>
              ) : (
                "Katkı Ekle"
              )}
            </Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  );
}
