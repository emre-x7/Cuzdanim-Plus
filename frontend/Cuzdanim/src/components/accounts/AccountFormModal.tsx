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
import { accountSchema, type AccountFormData } from "@/lib/validations";
import { AccountType, Currency } from "@/types";
import type { Account } from "@/types";

interface AccountFormModalProps {
  open: boolean;
  onClose: () => void;
  onSubmit: (data: AccountFormData) => void;
  isLoading: boolean;
  account?: Account | null;
}

export function AccountFormModal({
  open,
  onClose,
  onSubmit,
  isLoading,
  account,
}: AccountFormModalProps) {
  const isEditMode = !!account;

  const form = useForm<AccountFormData>({
    resolver: zodResolver(accountSchema),
    defaultValues: {
      name: "",
      type: AccountType.BankAccount,
      initialBalance: 0,
      currency: Currency.TRY,
      bankName: "",
      iban: "",
      cardLastFourDigits: "",
      creditLimit: undefined,
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

  // Edit modunda form'u doldur
  useEffect(() => {
    if (account) {
      setValue("name", account.name);

      // Type mapping
      const accountTypeMap: Record<string, number> = {
        BankAccount: AccountType.BankAccount,
        CreditCard: AccountType.CreditCard,
        Cash: AccountType.Cash,
        Wallet: AccountType.Wallet,
        Investment: AccountType.Investment,
      };

      const currencyMap: Record<string, number> = {
        TRY: Currency.TRY,
        USD: Currency.USD,
        EUR: Currency.EUR,
        GBP: Currency.GBP,
        GOLD: Currency.GOLD,
      };

      setValue("type", accountTypeMap[account.type] || AccountType.BankAccount);
      setValue("initialBalance", account.balance);
      setValue("currency", currencyMap[account.currency] || Currency.TRY);
      setValue("bankName", account.bankName || "");
      setValue("iban", account.iban || "");
    } else {
      reset();
    }
  }, [account, setValue, reset]);

  const onSubmitHandler: SubmitHandler<AccountFormData> = (data) => {
    onSubmit(data);
  };
  return (
    <Dialog open={open} onOpenChange={onClose}>
      <DialogContent className="sm:max-w-[500px] max-h-[90vh] overflow-y-auto">
        {Object.keys(errors).length > 0 && (
          <div className="bg-red-50 border border-red-200 p-3 rounded mb-4">
            <p className="font-semibold text-red-700 mb-2">Form Hataları:</p>
            <pre className="text-xs text-red-600">
              {JSON.stringify(errors, null, 2)}
            </pre>
          </div>
        )}
        <DialogHeader>
          <DialogTitle>
            {isEditMode ? "Hesap Düzenle" : "Yeni Hesap Ekle"}
          </DialogTitle>
          <DialogDescription>
            {isEditMode
              ? "Hesap bilgilerini güncelleyin"
              : "Yeni bir hesap oluşturun"}
          </DialogDescription>
        </DialogHeader>

        <form onSubmit={handleSubmit(onSubmitHandler)} className="space-y-4">
          {/* Hesap Adı */}
          <div className="space-y-2">
            <Label htmlFor="name">Hesap Adı *</Label>
            <Input
              id="name"
              placeholder="Ziraat Bankası Vadesiz"
              {...register("name")}
              disabled={isLoading}
            />
            {errors.name && (
              <p className="text-sm text-destructive">{errors.name.message}</p>
            )}
          </div>

          {/* Hesap Tipi */}
          <div className="space-y-2">
            <Label htmlFor="type">Hesap Tipi *</Label>
            <Select
              value={selectedType?.toString()}
              onValueChange={(value) =>
                setValue("type", parseInt(value), { shouldValidate: true })
              }
              disabled={isLoading || isEditMode}
            >
              <SelectTrigger>
                <SelectValue placeholder="Hesap tipi seçin" />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value={AccountType.BankAccount.toString()}>
                  Banka Hesabı
                </SelectItem>
                <SelectItem value={AccountType.CreditCard.toString()}>
                  Kredi Kartı
                </SelectItem>
                <SelectItem value={AccountType.Cash.toString()}>
                  Nakit
                </SelectItem>
                <SelectItem value={AccountType.Wallet.toString()}>
                  Dijital Cüzdan
                </SelectItem>
                <SelectItem value={AccountType.Investment.toString()}>
                  Yatırım
                </SelectItem>
              </SelectContent>
            </Select>
            {errors.type && (
              <p className="text-sm text-destructive">{errors.type.message}</p>
            )}
          </div>

          {/* Başlangıç Bakiyesi ve Para Birimi */}
          <div className="grid grid-cols-2 gap-4">
            <div className="space-y-2">
              <Label htmlFor="initialBalance">Başlangıç Bakiyesi *</Label>
              <Input
                id="initialBalance"
                type="number"
                step="0.01"
                placeholder="0.00"
                {...register("initialBalance", { valueAsNumber: true })}
                disabled={isLoading || isEditMode}
              />
              {errors.initialBalance && (
                <p className="text-sm text-destructive">
                  {errors.initialBalance.message}
                </p>
              )}
              {/*  Kredi kartı için yardımcı mesaj */}
              {selectedType === AccountType.CreditCard && !isEditMode && (
                <p className="text-xs text-muted-foreground">
                  💡 Kredi kartında borç varsa negatif girin (örn: -5000)
                </p>
              )}
            </div>

            <div className="space-y-2">
              <Label htmlFor="currency">Para Birimi *</Label>
              <Select
                value={watch("currency")?.toString()}
                onValueChange={(value) =>
                  setValue("currency", parseInt(value), {
                    shouldValidate: true,
                  })
                }
                disabled={isLoading || isEditMode}
              >
                <SelectTrigger>
                  <SelectValue />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value={Currency.TRY.toString()}>
                    TRY (₺)
                  </SelectItem>
                  <SelectItem value={Currency.USD.toString()}>
                    USD ($)
                  </SelectItem>
                  <SelectItem value={Currency.EUR.toString()}>
                    EUR (€)
                  </SelectItem>
                  <SelectItem value={Currency.GBP.toString()}>
                    GBP (£)
                  </SelectItem>
                  <SelectItem value={Currency.GOLD.toString()}>
                    GOLD (gr)
                  </SelectItem>
                </SelectContent>
              </Select>
              {errors.currency && (
                <p className="text-sm text-destructive">
                  {errors.currency.message}
                </p>
              )}
            </div>
          </div>

          {/* Banka Adı (Opsiyonel) */}
          <div className="space-y-2">
            <Label htmlFor="bankName">
              Banka Adı{" "}
              <span className="text-muted-foreground">(Opsiyonel)</span>
            </Label>
            <Input
              id="bankName"
              placeholder="Ziraat Bankası"
              {...register("bankName")}
              disabled={isLoading}
            />
          </div>

          {/* IBAN (Opsiyonel - Banka hesabı için) */}
          {selectedType === AccountType.BankAccount && (
            <div className="space-y-2">
              <Label htmlFor="iban">
                IBAN <span className="text-muted-foreground">(Opsiyonel)</span>
              </Label>
              <Input
                id="iban"
                placeholder="TR330006100519786457841326"
                maxLength={26}
                {...register("iban")}
                disabled={isLoading}
              />
              {errors.iban && (
                <p className="text-sm text-destructive">
                  {errors.iban.message}
                </p>
              )}
            </div>
          )}

          {/* Kredi Kartı Alanları */}
          {selectedType === AccountType.CreditCard && (
            <>
              <div className="space-y-2">
                <Label htmlFor="cardLastFourDigits">
                  Kart Son 4 Hanesi{" "}
                  <span className="text-muted-foreground">(Opsiyonel)</span>
                </Label>
                <Input
                  id="cardLastFourDigits"
                  placeholder="1234"
                  maxLength={4}
                  {...register("cardLastFourDigits")}
                  disabled={isLoading}
                />
                {errors.cardLastFourDigits && (
                  <p className="text-sm text-destructive">
                    {errors.cardLastFourDigits.message}
                  </p>
                )}
              </div>

              <div className="space-y-2">
                <Label htmlFor="creditLimit">
                  Kredi Limiti{" "}
                  <span className="text-muted-foreground">(Opsiyonel)</span>
                </Label>
                <Input
                  id="creditLimit"
                  type="number"
                  step="0.01"
                  placeholder="0.00"
                  {...register("creditLimit", {
                    valueAsNumber: true,
                    setValueAs: (v) => (v === "" ? undefined : Number(v)),
                  })}
                  disabled={isLoading}
                />
                {errors.creditLimit && (
                  <p className="text-sm text-destructive">
                    {errors.creditLimit.message}
                  </p>
                )}
              </div>
            </>
          )}

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
