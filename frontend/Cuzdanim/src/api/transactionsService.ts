import { api } from "@/lib/axios";
import type {
  ApiResult,
  Transaction,
  CreateTransactionRequest,
  UpdateTransactionRequest,
  GetTransactionsQuery,
} from "@/types";

export const transactionsService = {
  // Tarih aralığına göre işlemleri getir
  getTransactions: async (
    params: GetTransactionsQuery
  ): Promise<ApiResult<Transaction[]>> => {
    const response = await api.get<ApiResult<Transaction[]>>("/transactions", {
      params,
    });
    return response.data;
  },

  // ID ile işlem getir
  getTransactionById: async (id: string): Promise<ApiResult<Transaction>> => {
    const response = await api.get<ApiResult<Transaction>>(
      `/transactions/${id}`
    );
    return response.data;
  },

  // Yeni işlem oluştur
  createTransaction: async (
    data: CreateTransactionRequest
  ): Promise<ApiResult<string>> => {
    const currencyMap: Record<string, number> = {
      TRY: 1,
      USD: 2,
      EUR: 3,
      GBP: 4,
      GOLD: 5,
    };

    const payload = {
      type: data.type,
      amount: data.amount,
      accountId: data.accountId,
      categoryId: data.categoryId,
      transactionDate: data.transactionDate,
      currency:
        typeof data.currency === "string"
          ? currencyMap[data.currency] || 1
          : data.currency,
      description: data.description || undefined,
      notes: data.notes || undefined,
    };

    const response = await api.post<ApiResult<string>>(
      "/transactions",
      payload
    );
    return response.data;
  },

  //  İşlem güncelle
  updateTransaction: async (
    id: string,
    data: UpdateTransactionRequest
  ): Promise<ApiResult<string>> => {
    const currencyMap: Record<string, number> = {
      TRY: 1,
      USD: 2,
      EUR: 3,
      GBP: 4,
      GOLD: 5,
    };

    const payload = {
      amount: data.amount,
      categoryId: data.categoryId,
      transactionDate: data.transactionDate,
      currency:
        typeof data.currency === "string"
          ? currencyMap[data.currency] || 1
          : data.currency,
      description: data.description || undefined,
      notes: data.notes || undefined,
    };

    const response = await api.put<ApiResult<string>>(
      `/transactions/${id}`,
      payload
    );
    return response.data;
  },

  //  İşlem sil (soft delete)
  deleteTransaction: async (id: string): Promise<ApiResult<void>> => {
    const response = await api.delete<ApiResult<void>>(`/transactions/${id}`);
    return response.data;
  },
};
