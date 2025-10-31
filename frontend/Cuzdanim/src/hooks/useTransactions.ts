import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { transactionsService } from "@/api";
import { toast } from "sonner";
import type {
  CreateTransactionRequest,
  UpdateTransactionRequest,
  GetTransactionsQuery,
} from "@/types";

/**
 * Tarih aralığına göre işlemleri getir
 */
export function useTransactions(params: GetTransactionsQuery) {
  return useQuery({
    queryKey: ["transactions", params],
    queryFn: async () => {
      const response = await transactionsService.getTransactions(params);
      if (!response.isSuccess) {
        throw new Error(response.message);
      }
      return response.data || [];
    },
  });
}

/**
 * ID ile işlem getir
 */
export function useTransaction(id: string) {
  return useQuery({
    queryKey: ["transactions", id],
    queryFn: async () => {
      const response = await transactionsService.getTransactionById(id);
      if (!response.isSuccess) {
        throw new Error(response.message);
      }
      return response.data;
    },
    enabled: !!id,
  });
}

/**
 * Yeni işlem oluştur
 */
export function useCreateTransaction() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: CreateTransactionRequest) =>
      transactionsService.createTransaction(data),
    onSuccess: (response) => {
      queryClient.invalidateQueries({ queryKey: ["transactions"] });
      queryClient.invalidateQueries({ queryKey: ["accounts"] });
      queryClient.invalidateQueries({ queryKey: ["dashboard"] });
      queryClient.invalidateQueries({ queryKey: ["budgets"] });
      toast.success("Başarılı!", {
        description: response.message || "İşlem oluşturuldu",
      });
    },
    onError: (error: any) => {
      toast.error("Hata!", {
        description: error.response?.data?.message || "İşlem oluşturulamadı",
      });
    },
  });
}

/**
 * İşlem güncelle
 */
export function useUpdateTransaction() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({
      id,
      data,
    }: {
      id: string;
      data: UpdateTransactionRequest;
    }) => transactionsService.updateTransaction(id, data),
    onSuccess: (response) => {
      queryClient.invalidateQueries({ queryKey: ["transactions"] });
      queryClient.invalidateQueries({ queryKey: ["accounts"] });
      queryClient.invalidateQueries({ queryKey: ["dashboard"] });
      queryClient.invalidateQueries({ queryKey: ["budgets"] });
      toast.success("Başarılı!", {
        description: response.message || "İşlem güncellendi",
      });
    },
    onError: (error: any) => {
      toast.error("Hata!", {
        description: error.response?.data?.message || "İşlem güncellenemedi",
      });
    },
  });
}

/**
 * İşlem sil
 */
export function useDeleteTransaction() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (id: string) => transactionsService.deleteTransaction(id),
    onSuccess: (response) => {
      queryClient.invalidateQueries({ queryKey: ["transactions"] });
      queryClient.invalidateQueries({ queryKey: ["accounts"] });
      queryClient.invalidateQueries({ queryKey: ["dashboard"] });
      queryClient.invalidateQueries({ queryKey: ["budgets"] });
      toast.success("Başarılı!", {
        description: response.message || "İşlem silindi",
      });
    },
    onError: (error: any) => {
      toast.error("Hata!", {
        description: error.response?.data?.message || "İşlem silinemedi",
      });
    },
  });
}
