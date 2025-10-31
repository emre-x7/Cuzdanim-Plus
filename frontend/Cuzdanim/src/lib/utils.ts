import { clsx, type ClassValue } from "clsx";
import { twMerge } from "tailwind-merge";

export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs));
}

/**
 * Para formatı (Türkçe)
 */
export function formatCurrency(
  amount: number,
  currency: string = "TRY"
): string {
  const currencySymbols: Record<string, string> = {
    TRY: "₺",
    USD: "$",
    EUR: "€",
    GBP: "£",
    GOLD: "gr",
  };

  const symbol = currencySymbols[currency] || currency;
  const formatted = amount.toLocaleString("tr-TR", {
    minimumFractionDigits: 2,
    maximumFractionDigits: 2,
  });

  return `${formatted} ${symbol}`;
}

/**
 * Kısa para formatı (1.2K, 3.5M)
 */
export function formatCurrencyShort(
  amount: number,
  currency: string = "TRY"
): string {
  const currencySymbols: Record<string, string> = {
    TRY: "₺",
    USD: "$",
    EUR: "€",
    GBP: "£",
    GOLD: "gr",
  };

  const symbol = currencySymbols[currency] || currency;

  if (amount >= 1_000_000) {
    return `${(amount / 1_000_000).toFixed(1)}M ${symbol}`;
  }
  if (amount >= 1_000) {
    return `${(amount / 1_000).toFixed(1)}K ${symbol}`;
  }
  return `${amount.toFixed(0)} ${symbol}`;
}
