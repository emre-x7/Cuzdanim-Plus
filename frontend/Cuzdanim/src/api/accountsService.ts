import { api } from "@/lib/axios";
import type {
  ApiResult,
  Account,
  CreateAccountRequest,
  UpdateAccountRequest,
} from "@/types";

export const accountsService = {
  //  Kullanıcının tüm hesaplarını getir
  getAccounts: async (): Promise<ApiResult<Account[]>> => {
    const response = await api.get<ApiResult<Account[]>>("/accounts");
    return response.data;
  },

  //  Yeni hesap oluştur
  createAccount: async (
    data: CreateAccountRequest
  ): Promise<ApiResult<string>> => {
    const response = await api.post<ApiResult<string>>("/accounts", data);
    return response.data;
  },

  // Hesap güncelle
  updateAccount: async (
    id: string,
    data: UpdateAccountRequest
  ): Promise<ApiResult<string>> => {
    const response = await api.put<ApiResult<string>>(`/accounts/${id}`, data);
    return response.data;
  },

  // Hesap sil (soft delete)
  deleteAccount: async (id: string): Promise<ApiResult<void>> => {
    const response = await api.delete<ApiResult<void>>(`/accounts/${id}`);
    return response.data;
  },
};
