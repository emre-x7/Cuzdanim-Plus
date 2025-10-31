import { api } from "@/lib/axios";
import type {
  ApiResult,
  Budget,
  CreateBudgetRequest,
  UpdateBudgetRequest,
  GetBudgetsQuery,
} from "@/types";

export const budgetsService = {
  // Kullanıcının bütçelerini getir
  getBudgets: async (
    params?: GetBudgetsQuery
  ): Promise<ApiResult<Budget[]>> => {
    const response = await api.get<ApiResult<Budget[]>>("/budgets", { params });
    return response.data;
  },

  // ID ile bütçe getir
  getBudgetById: async (id: string): Promise<ApiResult<Budget>> => {
    const response = await api.get<ApiResult<Budget>>(`/budgets/${id}`);
    return response.data;
  },

  //  Yeni bütçe oluştur
  createBudget: async (
    data: CreateBudgetRequest
  ): Promise<ApiResult<string>> => {
    const response = await api.post<ApiResult<string>>("/budgets", data);
    return response.data;
  },

  // Bütçe güncelle
  updateBudget: async (
    id: string,
    data: UpdateBudgetRequest
  ): Promise<ApiResult<string>> => {
    const response = await api.put<ApiResult<string>>(`/budgets/${id}`, data);
    return response.data;
  },

  // Bütçe sil (soft delete)
  deleteBudget: async (id: string): Promise<ApiResult<void>> => {
    const response = await api.delete<ApiResult<void>>(`/budgets/${id}`);
    return response.data;
  },
};
