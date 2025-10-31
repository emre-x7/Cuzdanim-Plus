import { api } from "@/lib/axios";
import type {
  ApiResult,
  LoginRequest,
  LoginResponse,
  RegisterRequest,
  RefreshTokenRequest,
} from "@/types";

export const authService = {
  //  Kullanıcı girişi
  login: async (data: LoginRequest): Promise<ApiResult<LoginResponse>> => {
    const response = await api.post<ApiResult<LoginResponse>>(
      "/auth/login",
      data
    );
    return response.data;
  },

  // Kullanıcı kaydı
  register: async (
    data: RegisterRequest
  ): Promise<ApiResult<{ userId: string; email: string; message: string }>> => {
    const response = await api.post("/auth/register", data);
    return response.data;
  },

  // Token yenileme
  refreshToken: async (
    data: RefreshTokenRequest
  ): Promise<ApiResult<LoginResponse>> => {
    const response = await api.post<ApiResult<LoginResponse>>(
      "/auth/refresh-token",
      data
    );
    return response.data;
  },
};
