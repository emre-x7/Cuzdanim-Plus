import {
  Pencil,
  Trash2,
  Wallet,
  CreditCard,
  Banknote,
  Smartphone,
  TrendingUp,
} from "lucide-react";
import { Card, CardContent } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import { formatCurrency } from "@/lib/utils";
import type { Account } from "@/types";

interface AccountCardProps {
  account: Account;
  onEdit: (account: Account) => void;
  onDelete: (account: Account) => void;
}

// Hesap tipi icon mapping
const accountTypeIcons = {
  BankAccount: Wallet,
  CreditCard: CreditCard,
  Cash: Banknote,
  Wallet: Smartphone,
  Investment: TrendingUp,
};

// Hesap tipi renk mapping
const accountTypeColors = {
  BankAccount: "bg-blue-100 text-blue-700",
  CreditCard: "bg-purple-100 text-purple-700",
  Cash: "bg-green-100 text-green-700",
  Wallet: "bg-orange-100 text-orange-700",
  Investment: "bg-indigo-100 text-indigo-700",
};

// Hesap tipi label mapping
const accountTypeLabels = {
  BankAccount: "Banka Hesabı",
  CreditCard: "Kredi Kartı",
  Cash: "Nakit",
  Wallet: "Dijital Cüzdan",
  Investment: "Yatırım",
};

export function AccountCard({ account, onEdit, onDelete }: AccountCardProps) {
  const Icon =
    accountTypeIcons[account.type as keyof typeof accountTypeIcons] || Wallet;
  const colorClass =
    accountTypeColors[account.type as keyof typeof accountTypeColors] ||
    "bg-gray-100 text-gray-700";
  const typeLabel =
    accountTypeLabels[account.type as keyof typeof accountTypeLabels] ||
    account.type;

  return (
    <Card className="group hover:shadow-lg transition-shadow">
      <CardContent className="p-6">
        <div className="space-y-4">
          {/* Header: Icon + Badge */}
          <div className="flex items-start justify-between">
            <div
              className={`w-12 h-12 rounded-lg flex items-center justify-center ${colorClass}`}
            >
              <Icon className="w-6 h-6" />
            </div>
            <Badge variant={account.isActive ? "default" : "secondary"}>
              {account.isActive ? "Aktif" : "Pasif"}
            </Badge>
          </div>

          {/* Hesap Bilgileri */}
          <div className="space-y-1">
            <h3 className="font-semibold text-lg">{account.name}</h3>
            {account.bankName && (
              <p className="text-sm text-muted-foreground">
                {account.bankName}
              </p>
            )}
            <Badge variant="outline" className="text-xs">
              {typeLabel}
            </Badge>
          </div>

          {/* Bakiye */}
          <div className="pt-2 border-t">
            <p className="text-sm text-muted-foreground">Bakiye</p>
            <p className="text-2xl font-bold">
              {formatCurrency(account.balance, account.currency)}
            </p>
          </div>

          {/* Actions (Hover'da görünür) */}
          <div className="flex gap-2 opacity-0 group-hover:opacity-100 transition-opacity">
            <Button
              variant="outline"
              size="sm"
              className="flex-1"
              onClick={() => onEdit(account)}
            >
              <Pencil className="w-4 h-4 mr-2" />
              Düzenle
            </Button>
            <Button
              variant="outline"
              size="sm"
              className="text-destructive hover:bg-destructive hover:text-destructive-foreground"
              onClick={() => onDelete(account)}
            >
              <Trash2 className="w-4 h-4" />
            </Button>
          </div>
        </div>
      </CardContent>
    </Card>
  );
}
