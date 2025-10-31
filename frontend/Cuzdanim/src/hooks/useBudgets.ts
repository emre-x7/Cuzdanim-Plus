import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { budgetsService } from "@/api";
import { toast } from "sonner";
import type {
  CreateBudgetRequest,
  UpdateBudgetRequest,
  GetBudgetsQuery,
} from "@/types";

/**
 * Bütçeleri getir
 */
export function useBudgets(params?: GetBudgetsQuery) {
  return useQuery({
    queryKey: ["budgets", params],
    queryFn: async () => {
      const response = await budgetsService.getBudgets(params);
      if (!response.isSuccess) {
        throw new Error(response.message);
      }
      return response.data || [];
    },
  });
}

/**
 * ID ile bütçe getir
 */
export function useBudget(id: string) {
  return useQuery({
    queryKey: ["budgets", id],
    queryFn: async () => {
      const response = await budgetsService.getBudgetById(id);
      if (!response.isSuccess) {
        throw new Error(response.message);
      }
      return response.data;
    },
    enabled: !!id,
  });
}

/**
 * Yeni bütçe oluştur
 */
export function useCreateBudget() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: CreateBudgetRequest) =>
      budgetsService.createBudget(data),
    onSuccess: (response) => {
      queryClient.invalidateQueries({ queryKey: ["budgets"] });
      queryClient.invalidateQueries({ queryKey: ["dashboard"] });
      toast.success("Başarılı!", {
        description: response.message || "Bütçe oluşturuldu",
      });
    },
    onError: (error: any) => {
      toast.error("Hata!", {
        description: error.response?.data?.message || "Bütçe oluşturulamadı",
      });
    },
  });
}

/**
 * Bütçe güncelle
 */
export function useUpdateBudget() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ id, data }: { id: string; data: UpdateBudgetRequest }) =>
      budgetsService.updateBudget(id, data),
    onSuccess: (response) => {
      queryClient.invalidateQueries({ queryKey: ["budgets"] });
      queryClient.invalidateQueries({ queryKey: ["dashboard"] });
      toast.success("Başarılı!", {
        description: response.message || "Bütçe güncellendi",
      });
    },
    onError: (error: any) => {
      toast.error("Hata!", {
        description: error.response?.data?.message || "Bütçe güncellenemedi",
      });
    },
  });
}

/**
 * Bütçe sil
 */
export function useDeleteBudget() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (id: string) => budgetsService.deleteBudget(id),
    onSuccess: (response) => {
      queryClient.invalidateQueries({ queryKey: ["budgets"] });
      queryClient.invalidateQueries({ queryKey: ["dashboard"] });
      toast.success("Başarılı!", {
        description: response.message || "Bütçe silindi",
      });
    },
    onError: (error: any) => {
      toast.error("Hata!", {
        description: error.response?.data?.message || "Bütçe silinemedi",
      });
    },
  });
}
