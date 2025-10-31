import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { goalsService } from "@/api";
import { toast } from "sonner";
import type {
  CreateGoalRequest,
  UpdateGoalRequest,
  AddGoalContributionRequest,
} from "@/types";

/**
 * Hedefleri getir
 */
export function useGoals() {
  return useQuery({
    queryKey: ["goals"],
    queryFn: async () => {
      const response = await goalsService.getGoals();
      if (!response.isSuccess) {
        throw new Error(response.message);
      }
      return response.data || [];
    },
  });
}

/**
 * ID ile hedef getir
 */
export function useGoal(id: string) {
  return useQuery({
    queryKey: ["goals", id],
    queryFn: async () => {
      const response = await goalsService.getGoalById(id);
      if (!response.isSuccess) {
        throw new Error(response.message);
      }
      return response.data;
    },
    enabled: !!id,
  });
}

/**
 * Yeni hedef oluştur
 */
export function useCreateGoal() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: CreateGoalRequest) => goalsService.createGoal(data),
    onSuccess: (response) => {
      queryClient.invalidateQueries({ queryKey: ["goals"] });
      queryClient.invalidateQueries({ queryKey: ["dashboard"] });
      toast.success("Başarılı!", {
        description: response.message || "Hedef oluşturuldu",
      });
    },
    onError: (error: any) => {
      toast.error("Hata!", {
        description: error.response?.data?.message || "Hedef oluşturulamadı",
      });
    },
  });
}

/**
 * Hedef güncelle
 */
export function useUpdateGoal() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ id, data }: { id: string; data: UpdateGoalRequest }) =>
      goalsService.updateGoal(id, data),
    onSuccess: (response) => {
      queryClient.invalidateQueries({ queryKey: ["goals"] });
      queryClient.invalidateQueries({ queryKey: ["dashboard"] });
      toast.success("Başarılı!", {
        description: response.message || "Hedef güncellendi",
      });
    },
    onError: (error: any) => {
      toast.error("Hata!", {
        description: error.response?.data?.message || "Hedef güncellenemedi",
      });
    },
  });
}

/**
 * Hedef sil
 */
export function useDeleteGoal() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (id: string) => goalsService.deleteGoal(id),
    onSuccess: (response) => {
      queryClient.invalidateQueries({ queryKey: ["goals"] });
      queryClient.invalidateQueries({ queryKey: ["dashboard"] });
      toast.success("Başarılı!", {
        description: response.message || "Hedef silindi",
      });
    },
    onError: (error: any) => {
      toast.error("Hata!", {
        description: error.response?.data?.message || "Hedef silinemedi",
      });
    },
  });
}

/**
 * Hedefe katkı ekle
 */
export function useAddContribution() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({
      id,
      data,
    }: {
      id: string;
      data: AddGoalContributionRequest;
    }) => goalsService.addContribution(id, data),
    onSuccess: (response) => {
      queryClient.invalidateQueries({ queryKey: ["goals"] });
      queryClient.invalidateQueries({ queryKey: ["dashboard"] });
      queryClient.invalidateQueries({ queryKey: ["accounts"] }); // Hesap bakiyesi değişir
      toast.success("Başarılı!", {
        description: response.message || "Katkı eklendi",
      });
    },
    onError: (error: any) => {
      toast.error("Hata!", {
        description: error.response?.data?.message || "Katkı eklenemedi",
      });
    },
  });
}
