import { api } from "@/lib/axios";
import type {
  ApiResult,
  Goal,
  CreateGoalRequest,
  UpdateGoalRequest,
  AddGoalContributionRequest,
} from "@/types";

export const goalsService = {
  //  Kullanıcının hedeflerini getir
  getGoals: async (): Promise<ApiResult<Goal[]>> => {
    const response = await api.get<ApiResult<Goal[]>>("/goals");
    return response.data;
  },

  // ID ile hedef getir
  getGoalById: async (id: string): Promise<ApiResult<Goal>> => {
    const response = await api.get<ApiResult<Goal>>(`/goals/${id}`);
    return response.data;
  },

  // Yeni hedef oluştur
  createGoal: async (data: CreateGoalRequest): Promise<ApiResult<string>> => {
    const response = await api.post<ApiResult<string>>("/goals", data);
    return response.data;
  },

  // Hedef güncelle
  updateGoal: async (
    id: string,
    data: UpdateGoalRequest
  ): Promise<ApiResult<string>> => {
    const response = await api.put<ApiResult<string>>(`/goals/${id}`, data);
    return response.data;
  },

  // Hedef sil (soft delete)
  deleteGoal: async (id: string): Promise<ApiResult<void>> => {
    const response = await api.delete<ApiResult<void>>(`/goals/${id}`);
    return response.data;
  },

  // Hedefe katkı ekle
  addContribution: async (
    id: string,
    data: AddGoalContributionRequest
  ): Promise<ApiResult<void>> => {
    const response = await api.post<ApiResult<void>>(
      `/goals/${id}/contribute`,
      data
    );
    return response.data;
  },
};
